using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Operator logic.
    /// </summary>
    public class OperatorController
    {
        private OperatorDataAccess _OperatorDA;

        /// <summary>
        /// Creates a new OperatorController.
        /// </summary>
        public OperatorController()
        {
            this._OperatorDA = new OperatorDataAccess();
        }

        /// <summary>
        /// Retrieves a list of operators based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of operators.</returns>
        public GetResponse<Operator> GetOperators(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);

            // By default search only active operators.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Operator> response = new GetResponse<Operator>();
            DataTable table = _OperatorDA.GetOperators(request);
            int count = _OperatorDA.GetOperatorsCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Operator.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get an operator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The operator.</returns>
        public Operator GetOperator(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            DataTable table = _OperatorDA.GetOperator(id, request);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Operador no encontrado");
            }

            return Operator.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Creates an operator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The user data.</returns>
        public Operator CreateOperator(int id, OperatorRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            DataTable table = _OperatorDA.CreateOperator(id, request.Period);

            return Operator.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Deletes an operator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteOperator(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            _OperatorDA.DeleteOperator(id);
        }
    }
}