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
    /// Provide access to the data related to Courses.
    /// </summary>
    public class CourseDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new CourseDataAccess.
        /// </summary>
        public CourseDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Get the amount of courses that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of courses that satifies the query</returns>
        public int GetCourseCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetCoursesCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the courses that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the courses data that satisfies the query.</returns>
        public DataTable GetCourses(QueryString request)
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

            return _Connection.executeStoredProcedure("sp_GetCourses", parameters);
        }

        /// <summary>
        /// Get a specific course.
        /// </summary>
        /// <param name="id">The course identification.</param>
        /// <returns>A DataTable that contains the course data.</returns>
        public DataTable GetCourse(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.VarChar, id);
            return _Connection.executeStoredProcedure("sp_GetCourse", parameters);
        }

        /// <summary>
        /// Creates a course.
        /// </summary>
        /// <param name="course">The course data.</param>
        /// <returns>A DataTable that contains the course data.</returns>
        public DataTable CreateCourse(Course course)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, course.Name);
            parameters[1] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, course.Code);

            return _Connection.executeStoredProcedure("sp_CreateCourse", parameters);
        }

        /// <summary>
        /// Updates a course.
        /// </summary>
        /// <param name="id">The course identification.</param>
        /// <param name="course">The course data.</param>
        /// <returns>A DataTable that contains the course data.</returns>
        public DataTable UpdateCourse(int id, Course course)
        {
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, course.Name);
            parameters[2] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, course.Code);
            parameters[3] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, course.State);

            return _Connection.executeStoredProcedure("sp_UpdateCourse", parameters);
        }

        /// <summary>
        /// Deletes a course.
        /// </summary>
        /// <param name="id">The course identification.</param>
        public void DeleteCourse(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            _Connection.executeStoredProcedure("sp_DeleteCourse", parameters);
        }
    }
}