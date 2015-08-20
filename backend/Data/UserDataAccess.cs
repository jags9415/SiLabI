using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Provides the access to the data related to Users.
    /// </summary>
    public class UserDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new UserDataAccess.
        /// </summary>
        public UserDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Retrieve a user based on a credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public DataTable Authenticate(string username, string password)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);
            parameters[1] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, password);
            return _Connection.executeStoredProcedure("sp_Authenticate", parameters);
        }

        /// <summary>
        /// Get the amount of users that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of users that satifies the query</returns>
        public int GetUsersCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetUsersCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the users that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the users data that satisfies the query.</returns>
        public DataTable GetUsers(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[1] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[2] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[3] = SqlUtilities.CreateParameter("@page", SqlDbType.Int, request.Page);
            parameters[4] = SqlUtilities.CreateParameter("@limit", SqlDbType.Int, request.Limit);

            return _Connection.executeStoredProcedure("sp_GetUsers", parameters);
        }

        /// <summary>
        /// Get a specific user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A DataTable that contains the user data.</returns>
        public DataTable GetUser(string username)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);
            return _Connection.executeStoredProcedure("sp_GetUser", parameters);
        }
    }
}