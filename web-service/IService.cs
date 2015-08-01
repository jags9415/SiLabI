using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace SiLabI
{
    /// <summary>
    /// The web service interface.
    /// </summary>
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        [ServiceKnownType(typeof(AuthenticationResponse))]
        [WebInvoke(Method = "POST",
            UriTemplate = "/authenticate",
            RequestFormat = WebMessageFormat.Json),
        Description("Authenticate a user in the service.")]
        BaseResponse authenticate(AuthenticationRequest request);
    }
}
