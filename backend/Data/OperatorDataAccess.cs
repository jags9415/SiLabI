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
    /// Provide access to the data related to Operators.
    /// </summary>
    public class OperatorDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new OperatorDataAccess.
        /// </summary>
        public OperatorDataAccess()
        {
            _Connection = new Connection();
        }

        /// <summary>
        /// Get the amount of operators that satifies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The amount of operators that satifies the query</returns>
        public int GetOperatorsCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            DataTable table = _Connection.executeStoredProcedure("sp_GetOperatorsCount", parameters);
            DataRow row = table.Rows[0];

            return table.Columns.Contains("count") ? Converter.ToInt32(row["count"]) : 0;
        }

        /// <summary>
        /// Get all the operators that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the operators data that satisfies the query.</returns>
        public DataTable GetOperators(QueryString request)
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

            return _Connection.executeStoredProcedure("sp_GetOperators", parameters);
        }

        /// <summary>
        /// Get a specific operator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <returns>A DataTable that contains the operator data.</returns>
        public DataTable GetOperator(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@operator_id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);
            return _Connection.executeStoredProcedure("sp_GetOperator", parameters);
        }

        /// <summary>
        /// Creates an operator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="period">The period.</param>
        public DataTable CreateOperator(int id, Period period)
        {
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = SqlUtilities.CreateParameter("@user_id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@period_value", SqlDbType.Int, period.Value);
            parameters[2] = SqlUtilities.CreateParameter("@period_type", SqlDbType.VarChar, period.Type);
            parameters[3] = SqlUtilities.CreateParameter("@period_year", SqlDbType.Int, period.Year);

            return _Connection.executeStoredProcedure("sp_CreateOperator", parameters);
        }

        /// <summary>
        /// Deletes an operator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="period">The period.</param>
        public void DeleteOperator(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@operator_id", SqlDbType.Int, id);

            _Connection.executeStoredProcedure("sp_DeleteOperator", parameters);
        }
    }
}