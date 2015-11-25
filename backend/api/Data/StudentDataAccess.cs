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
        private ConnectionGroup _connectionGroup;

        /// <summary>
        /// Creates a new StudentDataAccess.
        /// </summary>
        public StudentDataAccess()
        {
            _connectionGroup = ConnectionGroup.Instance;
        }

        public int GetCount(Dictionary<string, object> payload, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _connectionGroup.Get(payload["type"] as string).executeScalar("sp_GetStudentsCount", parameters);
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

            return _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetStudents", parameters);
        }

        public DataRow GetOne(Dictionary<string, object> payload, string username, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);
            
            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetStudentByUsername", parameters);
            if (table.Rows.Count == 0)
            {
                throw new BaseException(HttpStatusCode.BadRequest, "Estudiante no encontrado.");
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

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetStudent", parameters);
            if (table.Rows.Count == 0)
            {
                throw new BaseException(HttpStatusCode.BadRequest, "Estudiante no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(Dictionary<string, object> payload, object obj)
        {
            Student student = (obj as Student);
            SqlParameter[] parameters = new SqlParameter[9];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, student.Name);
            parameters[2] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, student.LastName1);
            parameters[3] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, student.LastName2);
            parameters[4] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, student.Gender);
            parameters[5] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, student.Username);
            parameters[6] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, student.Password);
            parameters[7] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, student.Email);
            parameters[8] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, student.Phone);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_CreateStudent", parameters);
            return table.Rows[0];
        }

        public DataRow Update(Dictionary<string, object> payload, int id, object obj)
        {
            Student student = (obj as Student);
            SqlParameter[] parameters = new SqlParameter[11];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, student.Name);
            parameters[3] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, student.LastName1);
            parameters[4] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, student.LastName2);
            parameters[5] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, student.Gender);
            parameters[6] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, student.Username);
            parameters[7] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, student.Password);
            parameters[8] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, student.Email);
            parameters[9] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, student.Phone);
            parameters[10] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, student.State);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_UpdateStudent", parameters);
            return table.Rows[0];
        }

        public void Delete(Dictionary<string, object> payload, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);

            _connectionGroup.Get(payload["type"] as string).executeNonQuery("sp_DeleteStudent", parameters);
        }
    }
}