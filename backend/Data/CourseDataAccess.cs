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
    /// Perform CRUD operations on the Courses table.
    /// </summary>
    public class CourseDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new CourseDataAccess.
        /// </summary>
        public CourseDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetCoursesCount", parameters);
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

            return _Connection.executeQuery("sp_GetCourses", parameters);
        }

        public DataRow GetOne(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.VarChar, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetCourse", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Curso no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object obj)
        {
            Course course = (obj as Course);
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, course.Name);
            parameters[1] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, course.Code);

            DataTable table = _Connection.executeQuery("sp_CreateCourse", parameters);
            return table.Rows[0];
        }

        public DataRow Update(int id, object obj)
        {
            Course course = (obj as Course);
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, course.Name);
            parameters[2] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, course.Code);
            parameters[3] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, course.State);

            DataTable table = _Connection.executeQuery("sp_UpdateCourse", parameters);
            return table.Rows[0];
        }

        public void Delete(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            _Connection.executeNonQuery("sp_DeleteCourse", parameters);
        }
    }
}