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
    /// Administrator logic.
    /// </summary>
    public class AdministratorController
    {
        private AdministratorDataAccess _AdminDA;

        /// <summary>
        /// Creates a new AdministratorController.
        /// </summary>
        public AdministratorController()
        {
            this._AdminDA = new AdministratorDataAccess();
        }

        /// <summary>
        /// Retrieves a list of administrators based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of administrators.</returns>
        public GetResponse<User> GetAdministrators(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Query.Exists(element => element.Alias == "state"))
            {
                Field field = new Field("States", "Name", "state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<User> response = new GetResponse<User>();
            DataTable table = _AdminDA.GetAdministrators(request);
            int count = _AdminDA.GetAdministratorsCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The administrator.</returns>
        public User GetAdministrator(int id, string token)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _AdminDA.GetAdministrator(id);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "El identificador ingresado no corresponde a un administrador");
            }

            return User.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Creates an administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The user data.</returns>
        public void CreateAdministrator(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }
            
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            _AdminDA.CreateAdministrator(id);
        }

        /// <summary>
        /// Deletes an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteAdministrator(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            _AdminDA.DeleteAdministrator(id);
        }
    }
}