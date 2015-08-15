using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Reflection;
using System.ServiceModel.Activation;
using SiLabI.Model;
using SiLabI.Controllers;
using System.Net;
using SiLabI.Model.Query;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service : IService
    {
        private AuthenticationController _AuthController;
        private AdministratorController _AdminController;

        public Service()
        {
            this._AuthController = new AuthenticationController();
            this._AdminController = new AdministratorController();
        }

        /*
         * AUTHENTICATION ENDPOINTS IMPLEMENTATION.
        */

        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            return _AuthController.Authenticate(request);
        }

        /*
         * ADMINISTRATOR ENDPOINTS IMPLEMENTATION.
        */

        public List<User> GetAdministrators(string token, string query, string page, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(User.ValidFields);
            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);
            return _AdminController.GetAdministrators(request);
        }

        public User GetAdministrator(string id, string token)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return _AdminController.GetAdministrator(num, token);
        }

        public User CreateAdministrator(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return _AdminController.CreateAdministrator(num, request);
        }

        public User DeleteAdministrator(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return _AdminController.DeleteAdministrator(num, request);
        }
    }
}
