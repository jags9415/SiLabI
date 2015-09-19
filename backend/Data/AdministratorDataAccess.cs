using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Provide access to the data related to Administrators.
    /// </summary>
    public class AdministratorDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new AdministratorDataAccess.
        /// </summary>
        public AdministratorDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Get the amount of administrators that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of administrators that satifies the query</returns>
        public int GetAdministratorsCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetAdministratorsCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the administrators that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the administrators data that satisfies the query.</returns>
        public DataTable GetAdministrators(QueryString request)
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

            return _Connection.executeStoredProcedure("sp_GetAdministrators", parameters);
        }

        /// <summary>
        /// Get a specific administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <returns>A DataTable that contains the administrator data.</returns>
        public DataTable GetAdministrator(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);
            return _Connection.executeStoredProcedure("sp_GetAdministrator", parameters);
        }

        /// <summary>
        /// Creates an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        public DataTable CreateAdministrator(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            return _Connection.executeStoredProcedure("sp_CreateAdministrator", parameters);
        }

        /// <summary>
        /// Deletes an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        public void DeleteAdministrator(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            _Connection.executeStoredProcedure("sp_DeleteAdministrator", parameters);
        }
    }
}