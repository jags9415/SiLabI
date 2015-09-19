using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Perform CRUD operations for Administrators.
    /// </summary>
    public class AdministratorController : IController<User>
    {
        private AdministratorDataAccess _AdminDA;

        /// <summary>
        /// Creates a new AdministratorController.
        /// </summary>
        public AdministratorController()
        {
            this._AdminDA = new AdministratorDataAccess();
        }

        public GetResponse<User> GetAll(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active administrators.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<User> response = new GetResponse<User>();
            DataTable table = _AdminDA.GetAll(request);
            int count = _AdminDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public User GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _AdminDA.GetOne(id, request);
            return User.Parse(row);
        }

        public User Create(BaseRequest request)
        {
            AdministratorRequest adminRequest = (request as AdministratorRequest);
            if (adminRequest == null || !adminRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(adminRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            DataRow row = _AdminDA.Create(adminRequest.Id);
            return User.Parse(row);
        }

        public User Update(int id, BaseRequest request)
        {
            throw new InvalidOperationException("An administrator cannot be updated.");
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            _AdminDA.Delete(id);
        }
    }
}