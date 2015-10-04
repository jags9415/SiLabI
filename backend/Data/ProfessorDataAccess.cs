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
    /// Perform CRUD operations on the Professors table.
    /// </summary>
    public class ProfessorDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new ProfessorDataAccess.
        /// </summary>
        public ProfessorDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(object requesterId, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetProfessorsCount", parameters);
            return Converter.ToInt32(count);
        }

        public DataTable GetAll(object requesterId, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[6];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);

            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[2] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[3] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[4] = SqlUtilities.CreateParameter("@page", SqlDbType.Int, request.Page);
            parameters[5] = SqlUtilities.CreateParameter("@limit", SqlDbType.Int, request.Limit);

            return _Connection.executeQuery("sp_GetProfessors", parameters);
        }

        public DataRow GetOne(object requesterId, string username, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);
            
            DataTable table = _Connection.executeQuery("sp_GetProfessorByUsername", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Docente no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow GetOne(object requesterId, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetProfessor", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Docente no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object requesterId, object obj)
        {
            User professor = (obj as User);
            SqlParameter[] parameters = new SqlParameter[9];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, professor.Name);
            parameters[2] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, professor.LastName1);
            parameters[3] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, professor.LastName2);
            parameters[4] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, professor.Gender);
            parameters[5] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, professor.Username);
            parameters[6] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, professor.Password);
            parameters[7] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, professor.Email);
            parameters[8] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, professor.Phone);

            DataTable table = _Connection.executeQuery("sp_CreateProfessor", parameters);
            return table.Rows[0];
        }

        public DataRow Update(object requesterId, int id, object obj)
        {
            User professor = (obj as User);
            SqlParameter[] parameters = new SqlParameter[11];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, professor.Name);
            parameters[3] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, professor.LastName1);
            parameters[4] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, professor.LastName2);
            parameters[5] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, professor.Gender);
            parameters[6] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, professor.Username);
            parameters[7] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, professor.Password);
            parameters[8] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, professor.Email);
            parameters[9] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, professor.Phone);
            parameters[10] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, professor.State);

            DataTable table = _Connection.executeQuery("sp_UpdateProfessor", parameters);
            return table.Rows[0];
        }

        public void Delete(object requesterId, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);

            _Connection.executeNonQuery("sp_DeleteProfessor", parameters);
        }
    }
}