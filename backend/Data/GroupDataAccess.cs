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
    /// Perform CRUD operations on the Groups table.
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

        public int GetCount(object requesterId, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetGroupsCount", parameters);
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

            return _Connection.executeQuery("sp_GetGroups", parameters);
        }

        public DataTable GetAllByStudent(object requesterId, string student, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@student", SqlDbType.VarChar, student);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[3] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[4] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[4].Value = SqlUtilities.FormatWhereFields(request.Query);

            return _Connection.executeQuery("sp_GetGroupsByStudent", parameters);
        }

        public DataRow GetOne(object requesterId, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.VarChar, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetGroup", parameters);
            if (table.Rows.Count == 0)
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Grupo no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object requesterId, object obj)
        {
            InnerGroupRequest group = (obj as InnerGroupRequest);
            SqlParameter[] parameters;

            if (group.Students == null)
            {
                parameters = new SqlParameter[7];
            }
            else
            {
                parameters = new SqlParameter[8];
                DataTable students = createStudentTable(group.Students);
                parameters[7] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, students);
            }

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@course", SqlDbType.VarChar, group.Course);
            parameters[2] = SqlUtilities.CreateParameter("@number", SqlDbType.Int, group.Number);
            parameters[3] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, group.Professor);
            parameters[4] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, group.Period.Value);
            parameters[5] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, group.Period.Type);
            parameters[6] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, group.Period.Year);

            DataTable table = _Connection.executeQuery("sp_CreateGroup", parameters);
            return table.Rows[0];
        }

        public DataRow Update(object requesterId, int id, object obj)
        {
            InnerGroupRequest group = (obj as InnerGroupRequest);
            SqlParameter[] parameters;

            if (group.Students == null)
            {
                parameters = new SqlParameter[9];
            }
            else
            {
                parameters = new SqlParameter[10];
                DataTable students = createStudentTable(group.Students);
                parameters[9] = SqlUtilities.CreateParameter("@students", SqlDbType.Structured, students);
            }
            
            if (group.Period == null)
            {
                parameters[6] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, DBNull.Value);
                parameters[7] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, DBNull.Value);
                parameters[8] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, DBNull.Value);
            }
            else
            {
                parameters[6] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, group.Period.Value);
                parameters[7] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, group.Period.Type);
                parameters[8] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, group.Period.Year);
            }

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@course", SqlDbType.VarChar, group.Course);
            parameters[3] = SqlUtilities.CreateParameter("@number", SqlDbType.Int, group.Number);
            parameters[4] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, group.Professor);
            parameters[5] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, group.State);

            DataTable table = _Connection.executeQuery("sp_UpdateGroup", parameters);
            return table.Rows[0];
        }

        public void Delete(object requesterId, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            _Connection.executeNonQuery("sp_DeleteGroup", parameters);
        }

        private DataTable createStudentTable(List<string> usernames)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Username");
            usernames.ForEach(x => table.Rows.Add(x));
            return table;
        }
    }
}