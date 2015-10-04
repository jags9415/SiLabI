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
    /// perform CRUD operations on the StudentsByGroup table.
    /// </summary>
    public class StudentsByGroupDataAccess : IDataAccessIntermediate
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new StudentsByGroupDataAccess.
        /// </summary>
        public StudentsByGroupDataAccess()
        {
            _Connection = new Connection();
        }

        public DataTable GetAll(object requesterId, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[3] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[4] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[4].Value = SqlUtilities.FormatWhereFields(request.Query);

            return _Connection.executeQuery("sp_GetStudentsByGroup", parameters);
        }

        public void Create(object requesterId, int id, object obj)
        {
            List<string> students = (obj as List<string>);
            SqlParameter[] parameters = new SqlParameter[3];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, table);

            _Connection.executeNonQuery("sp_AddStudentsToGroup", parameters);
        }

        public void Update(object requesterId, int id, object obj)
        {
            List<string> students = (obj as List<string>);
            SqlParameter[] parameters = new SqlParameter[3];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, table);

            _Connection.executeNonQuery("sp_UpdateGroupStudents", parameters);
        }

        public void Delete(object requesterId, int id, object obj)
        {
            List<string> students = (obj as List<string>);
            SqlParameter[] parameters = new SqlParameter[3];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, table);

            _Connection.executeNonQuery("sp_RemoveStudentsFromGroup", parameters);
        }
    }
}