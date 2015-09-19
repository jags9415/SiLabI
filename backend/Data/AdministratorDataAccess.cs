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
using System.Text;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Perform CRUD operations on the Administrators table.
    /// </summary>
    public class AdministratorDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new AdministratorDataAccess.
        /// </summary>
        public AdministratorDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetAdministratorsCount", parameters);
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

            return _Connection.executeQuery("sp_GetAdministrators", parameters);
        }

        public DataRow GetOne(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);
            
            DataTable table = _Connection.executeQuery("sp_GetAdministrator", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Administrador no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object obj)
        {
            Int32? id = (obj as Int32?);
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            
            DataTable table = _Connection.executeQuery("sp_CreateAdministrator", parameters);
            return table.Rows[0];
        }

        public DataRow Update(int id, object obj)
        {
            throw new InvalidOperationException("Cannot perform UPDATE operation on Administrators table.");
        }

        public void Delete(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            _Connection.executeNonQuery("sp_DeleteAdministrator", parameters);
        }
    }
}