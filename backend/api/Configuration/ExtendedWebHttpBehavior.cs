using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace SiLabI.Configuration
{
    // <summary>
    /// This class is a custom implementation of the WebHttpBehavior. 
    /// The main of this class is to handle exception and to serialize those as requests that will be understood by the web application.
    /// Also send a JSON error instead of 404 html when an endpoint is not found.
    /// </summary>
    public class ExtendedWebHttpBehavior : WebHttpBehavior
    {
        /// <summary>
        /// Clear all ErrorHandlers and add the ExtendedErrorHandler.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        protected override void AddServerErrorHandlers(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Clear();
            endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new ExtendedErrorHandler());
        }

        /// <summary>
        /// Add the UnknownOperationInvoker.
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public override void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            base.ApplyDispatchBehavior(endpoint, endpointDispatcher);

            endpointDispatcher.DispatchRuntime.Operations.Remove(endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation);
            endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation = new DispatchOperation(endpointDispatcher.DispatchRuntime, "*", "*", "*");
            endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation.DeserializeRequest = false;
            endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation.SerializeReply = false;
            endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation.Invoker = new UnknownOperationInvoker();
        }
    }
}