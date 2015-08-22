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
    /// Provide access to the data related to Professors.
    /// </summary>
    public class ProfessorDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new ProfessorDataAccess.
        /// </summary>
        public ProfessorDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Get the amount of professors that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of professors that satifies the query</returns>
        public int GetProfessorsCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetProfessorsCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the professors that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the professors data that satisfies the query.</returns>
        public DataTable GetProfessors(QueryString request)
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

            return _Connection.executeStoredProcedure("sp_GetProfessors", parameters);
        }

        /// <summary>
        /// Get a specific professor.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <returns>A DataTable that contains the professor data.</returns>
        public DataTable GetProfessor(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            return _Connection.executeStoredProcedure("sp_GetProfessor", parameters);
        }

        /// <summary>
        /// Creates a professor.
        /// </summary>
        /// <param name="professor">The professor data.</param>
        /// <returns>A DataTable that contains the professor data.</returns>
        public DataTable CreateProfessor(User professor)
        {
            SqlParameter[] parameters = new SqlParameter[8];

            parameters[0] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, professor.Name);
            parameters[1] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, professor.LastName1);
            parameters[2] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, professor.LastName2);
            parameters[3] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, professor.Gender);
            parameters[4] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, professor.Username);
            parameters[5] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, professor.Password);
            parameters[6] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, professor.Email);
            parameters[7] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, professor.Phone);

            return _Connection.executeStoredProcedure("sp_CreateProfessor", parameters);
        }

        /// <summary>
        /// Updates a professor.
        /// </summary>
        /// <param name="professor">The professor data.</param>
        /// <returns>A DataTable that contains the professor data.</returns>
        public DataTable UpdateProfessor(int id, User professor)
        {
            SqlParameter[] parameters = new SqlParameter[10];

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, professor.Name);
            parameters[2] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, professor.LastName1);
            parameters[3] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, professor.LastName2);
            parameters[4] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, professor.Gender);
            parameters[5] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, professor.Username);
            parameters[6] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, professor.Password);
            parameters[7] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, professor.Email);
            parameters[8] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, professor.Phone);
            parameters[9] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, professor.State);

            return _Connection.executeStoredProcedure("sp_UpdateProfessor", parameters);
        }

        /// <summary>
        /// Deletes a professor.
        /// </summary>
        /// <param name="id">The user identification.</param>
        public void DeleteProfessor(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            _Connection.executeStoredProcedure("sp_DeleteProfessor", parameters);
        }
    }
}