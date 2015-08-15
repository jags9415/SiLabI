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
        /*
         * AUTHENTICATION ENDPOINTS.
        */

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/authenticate",
            RequestFormat = WebMessageFormat.Json),
        Description("Authenticate a user in the service.")]
        AuthenticationResponse Authenticate(AuthenticationRequest request);

        /*
         * ADMINISTRATOR ENDPOINTS.
        */

        [OperationContract]
        [WebGet(UriTemplate = "/administrators?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}"),
        Description("Retrieve a list of administrators.")]
        List<User> GetAdministrators(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/administrators/{id}?access_token={token}"),
        Description("Retrieve an administrator.")]
        User GetAdministrator(string id, string token);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/administrators/{id}",
            RequestFormat = WebMessageFormat.Json),
        Description("Create an administrator.")]
        User CreateAdministrator(string id, BaseRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/administrators/{id}",
            RequestFormat = WebMessageFormat.Json),
        Description("Delete an administrators.")]
        User DeleteAdministrator(string id, BaseRequest request);
    }
}
