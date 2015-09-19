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
    /// User logic.
    /// </summary>
    public class UserController
    {
        private UserDataAccess _UserDA;

        /// <summary>
        /// Creates an UserController();
        /// </summary>
        public UserController()
        {
            this._UserDA = new UserDataAccess();
        }

        /// <summary>
        /// Retrieves a list of users based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of users.</returns>
        public GetResponse<User> GetUsers(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active users.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<User> response = new GetResponse<User>();
            DataTable table = _UserDA.GetUsers(request);
            int count = _UserDA.GetUsersCount(request);     

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get an user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The user.</returns>
        public User GetUser(string username, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _UserDA.GetUser(username, request);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Usuario no encontrado");
            }

            return User.Parse(table.Rows[0]);
        }
    }
}