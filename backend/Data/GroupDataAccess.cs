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
    /// Provide access to the data related to Groups.
    /// </summary>
    public class GroupDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new GroupDataAccess.
        /// </summary>
        public GroupDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Get the amount of groups that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of groups that satifies the query</returns>
        public int GetGroupsCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetGroupsCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the groups that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the groups data that satisfies the query.</returns>
        public DataTable GetGroups(QueryString request)
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

            return _Connection.executeStoredProcedure("sp_GetGroups", parameters);
        }

        /// <summary>
        /// Get a specific group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <returns>A DataTable that contains the group data.</returns>
        public DataTable GetGroup(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.VarChar, id);
            return _Connection.executeStoredProcedure("sp_GetGroup", parameters);
        }

        /// <summary>
        /// Creates a group.
        /// </summary>
        /// <param name="group">The group data.</param>
        /// <returns>A DataTable that contains the group data.</returns>
        public DataTable CreateGroup(InnerGroupRequest group)
        {
            SqlParameter[] parameters;

            if (group.Students == null)
            {
                parameters = new SqlParameter[6];
            }
            else
            {
                parameters = new SqlParameter[7];

                DataTable students = new DataTable();
                students.Columns.Add("Username");
                group.Students.ForEach(x => students.Rows.Add(x));

                parameters[6] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, students);
            }

            parameters[0] = SqlUtilities.CreateParameter("@course", SqlDbType.VarChar, group.Course);
            parameters[1] = SqlUtilities.CreateParameter("@number", SqlDbType.Int, group.Number);
            parameters[2] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, group.Professor);
            parameters[3] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, group.Period.Value);
            parameters[4] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, group.Period.Type);
            parameters[5] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, group.Period.Year);

            return _Connection.executeStoredProcedure("sp_CreateGroup", parameters);
        }

        /// <summary>
        /// Updates a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="group">The group data.</param>
        /// <returns>A DataTable that contains the group data.</returns>
        public DataTable UpdateGroup(int id, InnerGroupRequest group)
        {
            SqlParameter[] parameters;

            if (group.Students == null)
            {
                parameters = new SqlParameter[8];
            }
            else
            {
                parameters = new SqlParameter[9];

                DataTable students = new DataTable();
                students.Columns.Add("Username");
                group.Students.ForEach(x => students.Rows.Add(x));

                parameters[8] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, students);
            }
            
            if (group.Period == null)
            {
                parameters[4] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, DBNull.Value);
                parameters[5] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, DBNull.Value);
                parameters[6] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                parameters[4] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, group.Period.Value);
                parameters[5] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, group.Period.Type);
                parameters[6] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, group.Period.Year);
            }

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@course", SqlDbType.VarChar, group.Course);
            parameters[2] = SqlUtilities.CreateParameter("@number", SqlDbType.Int, group.Number);
            parameters[3] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, group.Professor);
            parameters[7] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, group.State);

            return _Connection.executeStoredProcedure("sp_UpdateGroup", parameters);
        }

        /// <summary>
        /// Deletes a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        public void DeleteGroup(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            _Connection.executeStoredProcedure("sp_DeleteGroup", parameters);
        }

        /// <summary>
        /// Add a list of students to a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="students">The list of students usernames.</param>
        public void AddStudentsToGroup(int id, List<string> students)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            
            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, table);

            _Connection.executeStoredProcedure("sp_AddStudentsToGroup", parameters);
        }

        /// <summary>
        /// Update a group students list.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="students">The list of students usernames.</param>
        public void UpdateGroupStudents(int id, List<string> students)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, table);

            _Connection.executeStoredProcedure("sp_UpdateGroupStudents", parameters);
        }

        /// <summary>
        /// Delete a list of students from a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="students">The list of students usernames.</param>
        public void DeleteStudentsFromGroup(int id, List<string> students)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, table);

            _Connection.executeStoredProcedure("sp_RemoveStudentsFromGroup", parameters);
        }

        /// <summary>
        /// Get the list of students of a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <returns>A DataTable with the records of the students.</returns>
        public DataTable GetGroupStudents(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, id);
            return _Connection.executeStoredProcedure("sp_GetGroupStudents", parameters);
        }
    }
}