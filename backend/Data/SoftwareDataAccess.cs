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
    /// Provide access to data related to software.
    /// </summary>
    public class SoftwareDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new CourseDataAccess.
        /// </summary>
        public SoftwareDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Get the amount of software that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of software that satifies the query</returns>
        public int GetSoftwareCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetSoftwaresCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the software that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the software data that satisfies the query.</returns>
        public DataTable GetSoftwares(QueryString request)
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

            return _Connection.executeStoredProcedure("sp_GetSoftwares", parameters);
        }

        /// <summary>
        /// Get a specific software.
        /// </summary>
        /// <param name="id">The software identification.</param>
        /// <returns>A DataTable that contains the software data.</returns>
        public DataTable GetSoftware(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.VarChar, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);
            return _Connection.executeStoredProcedure("sp_GetSoftware", parameters);
        }

        /// <summary>
        /// Creates a software.
        /// </summary>
        /// <param name="software">The software data.</param>
        /// <returns>A DataTable that contains the software data.</returns>
        public DataTable CreateSoftware(Software software)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, software.Name);
            parameters[1] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, software.Code);

            return _Connection.executeStoredProcedure("sp_CreateSoftware", parameters);
        }

        /// <summary>
        /// Updates a software.
        /// </summary>
        /// <param name="id">The software identification.</param>
        /// <param name="software">The software data.</param>
        /// <returns>A DataTable that contains the software data.</returns>
        public DataTable UpdateSoftware(int id, Software software)
        {
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, software.Name);
            parameters[2] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, software.Code);
            parameters[3] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, software.State);

            return _Connection.executeStoredProcedure("sp_UpdateSoftware", parameters);
        }

        /// <summary>
        /// Deletes a software.
        /// </summary>
        /// <param name="id">The software identification.</param>
        public void DeleteSoftware(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            _Connection.executeStoredProcedure("sp_DeleteSoftware", parameters);
        }
    }
}