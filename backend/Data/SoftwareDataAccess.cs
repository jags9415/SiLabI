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
    /// Provide access to data related to software.
    /// </summary>
    public class SoftwareDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new CourseDataAccess.
        /// </summary>
        public SoftwareDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(object requesterId, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetSoftwaresCount", parameters);
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

            return _Connection.executeQuery("sp_GetSoftwares", parameters);
        }

        public DataRow GetOne(object requesterId, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetSoftware", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Software no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow GetOne(object requesterId, string code, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, code);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetSoftwareByCode", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Software no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object requesterId, object obj)
        {
            Software software = (obj as Software);
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, software.Name);
            parameters[2] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, software.Code);

            DataTable table = _Connection.executeQuery("sp_CreateSoftware", parameters);
            return table.Rows[0];
        }

        public DataRow Update(object requesterId, int id, object obj)
        {
            Software software = (obj as Software);
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, software.Name);
            parameters[3] = SqlUtilities.CreateParameter("@code", SqlDbType.VarChar, software.Code);
            parameters[4] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, software.State);

            DataTable table = _Connection.executeQuery("sp_UpdateSoftware", parameters);
            return table.Rows[0];
        }

        public void Delete(object requesterId, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            
            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            _Connection.executeNonQuery("sp_DeleteSoftware", parameters);
        }
    }
}