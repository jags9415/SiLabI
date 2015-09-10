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
    /// Provide access to the data related to Students.
    /// </summary>
    public class StudentDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new StudentDataAccess.
        /// </summary>
        public StudentDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Get the amount of students that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of students that satifies the query</returns>
        public int GetStudentsCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetStudentsCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the students that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the students data that satisfies the query.</returns>
        public DataTable GetStudents(QueryString request)
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

            return _Connection.executeStoredProcedure("sp_GetStudents", parameters);
        }

        /// <summary>
        /// Get a specific student.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>A DataTable that contains the student data.</returns>
        public DataTable GetStudent(string username)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);
            return _Connection.executeStoredProcedure("sp_GetStudentByUsername", parameters);
        }

        /// <summary>
        /// Get a specific student.
        /// </summary>
        /// <param name="id">The student identification.</param>
        /// <returns>A DataTable that contains the student data.</returns>
        public DataTable GetProfessor(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            return _Connection.executeStoredProcedure("sp_GetStudent", parameters);
        }


        /// <summary>
        /// Creates a student.
        /// </summary>
        /// <param name="student">The student data.</param>
        /// <returns>A DataTable that contains the student data.</returns>
        public DataTable CreateStudent(Student student)
        {
            SqlParameter[] parameters = new SqlParameter[8];

            parameters[0] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, student.Name);
            parameters[1] = SqlUtilities.CreateParameter("@last_name_1", SqlDbType.VarChar, student.LastName1);
            parameters[2] = SqlUtilities.CreateParameter("@last_name_2", SqlDbType.VarChar, student.LastName2);
            parameters[3] = SqlUtilities.CreateParameter("@gender", SqlDbType.VarChar, student.Gender);
            parameters[4] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, student.Username);
            parameters[5] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, student.Password);
            parameters[6] = SqlUtilities.CreateParameter("@email", SqlDbType.VarChar, student.Email);
            parameters[7] = SqlUtilities.CreateParameter("@phone", SqlDbType.VarChar, student.Phone);

            return _Connection.executeStoredProcedure("sp_CreateStudent", parameters);
        }

        /// <summary>
        /// Updates a student.
        /// </summary>
        /// <param name="student">The student data.</param>
        /// <returns>A DataTable that contains the student data.</returns>
        public DataTable UpdateStudent(int id, Student student)
        {
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

            return _Connection.executeStoredProcedure("sp_UpdateStudent", parameters);
        }

        /// <summary>
        /// Deletes a student.
        /// </summary>
        /// <param name="id">The user identification.</param>
        public void DeleteStudent(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            _Connection.executeStoredProcedure("sp_DeleteStudent", parameters);
        }
    }
}