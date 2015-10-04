using SiLabI.Exceptions;
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
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Admin);
            QueryString request = new QueryString(ValidFields.Administrator);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _AdminController.GetAll(request, payload);
        }

        public User GetAdministrator(string id, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Admin);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Administrator);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _AdminController.GetOne(num, request, payload);
        }

        public User CreateAdministrator(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            AdministratorRequest adminRequest = new AdministratorRequest();
            adminRequest.Id = num;
            adminRequest.AccessToken = request.AccessToken;

            return _AdminController.Create(adminRequest, payload);
        }

        public void DeleteAdministrator(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _AdminController.Delete(num, request, payload);
        }
    }
}
