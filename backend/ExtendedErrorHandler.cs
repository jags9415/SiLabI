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

namespace SiLabI
{
    public class ExtendedErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return true;
        }

        public void ProvideFault(Exception exception, MessageVersion version, ref Message fault)
        {
            Error error;

            if (exception is WcfException)
            {
                error = (exception as WcfException).Error;
            }
            else
            {
                error = new Error(HttpStatusCode.InternalServerError, exception.GetType().ToString(), exception.Message);
            }

            fault = Message.CreateMessage(version, "", error, new DataContractJsonSerializer(typeof(Error)));

            // Tell WCF to use JSON encoding rather than default XML
            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

            // Modify response
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