using SiLabI.Exceptions;
using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace SiLabI.Configuration
{
    /// <summary>
    /// Handle the HTTP 404 NotFound and send a JSON error message.
    /// </summary>
    internal class UnknownOperationInvoker : IOperationInvoker
    {
        public object[] AllocateInputs()
        {
            return new object[1];
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            // Create message
            var error = new Error(HttpStatusCode.NotFound, "Resource not found.");
            var fault = Message.CreateMessage(MessageVersion.None, "", error, new DataContractJsonSerializer(typeof(Error)));

            // Tell WCF to use JSON encoding rather than default XML
            var wbf = new WebBodyFormatMessageProperty(WebContentFormat.Json);
            fault.Properties.Add(WebBodyFormatMessageProperty.Name, wbf);

            // Modify response
            var rmp = new HttpResponseMessageProperty
            {
                StatusCode = HttpStatusCode.NotFound,
                StatusDescription = "NotFound",
            };

            rmp.Headers[HttpResponseHeader.ContentType] = "application/json";
            fault.Properties.Add(HttpResponseMessageProperty.Name, rmp);

            outputs = new object[0];

            return fault;
        }

        public System.IAsyncResult InvokeBegin(object instance, object[] inputs, System.AsyncCallback callback, object state)
        {
            throw new System.NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, System.IAsyncResult result)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}