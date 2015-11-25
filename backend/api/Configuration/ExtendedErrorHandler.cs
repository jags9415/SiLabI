using SiLabI.Exceptions;
using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace SiLabI.Configuration
{
    /// <summary>
    /// Catch any runtime exception and wrap it into a JSON error.
    /// </summary>
    public class ExtendedErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Always handle the exception.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {
            return true;
        }

        /// <summary>
        /// Sends the JSON error.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
        public void ProvideFault(Exception exception, MessageVersion version, ref Message fault)
        {
            Error error;

            if (exception is BaseException)
            {
                error = (exception as BaseException).Error;
            }
            else
            {
                error = new Error(HttpStatusCode.InternalServerError, exception.GetType().ToString(), exception.Message);
            }

            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
            fault = Message.CreateMessage(version, "", error, new DataContractJsonSerializer(typeof(Error)));
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

            var rmp = new HttpResponseMessageProperty
            {
                StatusCode = error.Code,
                StatusDescription = error.Code.ToString(),
            };

            rmp.Headers[HttpResponseHeader.ContentType] = "application/json";
            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);
        }
    }
}