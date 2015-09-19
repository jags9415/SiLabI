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
    /// Provide access to the data related to Students.
    /// </summary>
    public class StudentDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new StudentDataAccess.
        /// </summary>
        public StudentDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetStudentsCount", parameters);
            return Converter.ToInt32(count);
        }

        public DataTable GetAll(QueryString request)
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

            return _Connection.executeQuery("sp_GetStudents", parameters);
        }

        public DataRow GetOne(string username, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetStudentByUsername", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Estudiante no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow GetOne(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetStudent", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Estudiante no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object obj)
        {
            Student student = (obj as Student);
            SqlParameter[] parameters = new SqlParameter[8];

            parameters[0] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, student.Name);
            parameters[1] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, student.LastName1);
            parameters[2] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, student.LastName2);
            parameters[3] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, student.Gender);
            parameters[4] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, student.Username);
            parameters[5] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, student.Password);
            parameters[6] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, student.Email);
            parameters[7] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, student.Phone);

            DataTable table = _Connection.executeQuery("sp_CreateStudent", parameters);
            return table.Rows[0];
        }

        public DataRow Update(int id, object obj)
        {
            Student student = (obj as Student);
            SqlParameter[] parameters = new SqlParameter[10];

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, student.Name);
            parameters[2] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, student.LastName1);
            parameters[3] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, student.LastName2);
            parameters[4] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, student.Gender);
            parameters[5] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, student.Username);
            parameters[6] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, student.Password);
            parameters[7] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, student.Email);
            parameters[8] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, student.Phone);
            parameters[9] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, student.State);

            DataTable table = _Connection.executeQuery("sp_UpdateStudent", parameters);
            return table.Rows[0];
        }

        public void Delete(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            _Connection.executeNonQuery("sp_DeleteStudent", parameters);
        }
    }
}