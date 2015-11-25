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
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Provides the access to the data related to Users.
    /// </summary>
    public class UserDataAccess : IDataAccess
    {
        private ConnectionGroup _connectionGroup;

        /// <summary>
        /// Creates a new UserDataAccess.
        /// </summary>
        public UserDataAccess()
        {
            _connectionGroup = ConnectionGroup.Instance;
        }

        public int GetCount(Dictionary<string, object> payload, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _connectionGroup.Get(payload["type"] as string).executeScalar("sp_GetUsersCount", parameters);
            return Converter.ToInt32(count);
        }

        public DataTable GetAll(Dictionary<string, object> payload, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[6];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[2] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[3] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[4] = SqlUtilities.CreateParameter("@page", SqlDbType.Int, request.Page);
            parameters[5] = SqlUtilities.CreateParameter("@limit", SqlDbType.Int, request.Limit);

            return _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetUsers", parameters);
        }

        public DataRow GetOne(Dictionary<string, object> payload, string username, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetUserByUsername", parameters);
            if (table.Rows.Count == 0)
            {
                throw new BaseException(HttpStatusCode.BadRequest, "Usuario no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow GetOne(Dictionary<string, object> payload, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetUser", parameters);
            if (table.Rows.Count == 0)
            {
                throw new BaseException(HttpStatusCode.BadRequest, "Usuario no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(Dictionary<string, object> payload, object obj)
        {
            throw new InvalidOperationException("Cannot perform CREATE operation on Users table.");
        }

        public DataRow Update(Dictionary<string, object> payload, int id, object obj)
        {
            User user = (obj as User);
            SqlParameter[] parameters = new SqlParameter[11];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, user.Name);
            parameters[3] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, user.LastName1);
            parameters[4] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, user.LastName2);
            parameters[5] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, user.Gender);
            parameters[6] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, user.Username);
            parameters[7] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, user.Password);
            parameters[8] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, user.Email);
            parameters[9] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, user.Phone);
            parameters[10] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, user.State);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_UpdateUser", parameters);
            return table.Rows[0];
        }

        public void Delete(Dictionary<string, object> payload, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);

            _connectionGroup.Get(payload["type"] as string).executeNonQuery("sp_DeleteUser", parameters);
        }
    }
}