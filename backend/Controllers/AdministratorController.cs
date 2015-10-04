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

        public GetResponse<User> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active administrators.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Administrator, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<User> response = new GetResponse<User>();
            DataTable table = _AdminDA.GetAll(payload["id"], request);
            int count = _AdminDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public User GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _AdminDA.GetOne(payload["id"], id, request);
            return User.Parse(row);
        }

        public User Create(BaseRequest request, Dictionary<string, object> payload)
        {
            AdministratorRequest adminRequest = (request as AdministratorRequest);
            if (adminRequest == null || !adminRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            DataRow row = _AdminDA.Create(payload["id"], adminRequest.Id);
            return User.Parse(row);
        }

        public User Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            throw new InvalidOperationException("An administrator cannot be updated.");
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _AdminDA.Delete(payload["id"], id);
        }
    }
}