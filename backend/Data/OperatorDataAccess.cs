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
    /// Provide access to the data related to Operators.
    /// </summary>
    public class OperatorDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new OperatorDataAccess.
        /// </summary>
        public OperatorDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetOperatorsCount", parameters);
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

            return _Connection.executeQuery("sp_GetOperators", parameters);
        }

        public DataRow GetOne(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@operator_id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetOperator", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Operador no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object obj)
        {
            OperatorRequest request = (obj as OperatorRequest);
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, request.Id);
            parameters[1] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, request.Period.Value);
            parameters[2] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, request.Period.Type);
            parameters[3] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, request.Period.Year);

            DataTable table = _Connection.executeQuery("sp_CreateOperator", parameters);
            return table.Rows[0];
        }

        public DataRow Update(int id, object obj)
        {
            throw new InvalidOperationException("Cannot perform UPDATE operation on Operators table.");
        }

        public void Delete(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@operator_id", SqlDbType.Int, id);

            _Connection.executeNonQuery("sp_DeleteOperator", parameters);
        }
    }
}