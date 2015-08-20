using SiLabI.Model;
using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        public GetResponse<User> GetAdministrators(string token, string query, string page, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(ValidFields.Administrator);

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
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return _AdminController.GetAdministrator(num, token);
        }

        public void CreateAdministrator(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
             _AdminController.CreateAdministrator(num, request);
        }

        public void DeleteAdministrator(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            _AdminController.DeleteAdministrator(num, request);
        }
    }
}
