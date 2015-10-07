using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Perform CRUD operations for Operators.
    /// </summary>
    public class OperatorController : IController<Operator>
    {
        private OperatorDataAccess _OperatorDA;

        /// <summary>
        /// Creates a new OperatorController.
        /// </summary>
        public OperatorController()
        {
            this._OperatorDA = new OperatorDataAccess();
        }

        public PaginatedResponse<Operator> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active operators.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Operator, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            PaginatedResponse<Operator> response = new PaginatedResponse<Operator>();
            DataTable table = _OperatorDA.GetAll(payload["id"], request);
            int count = _OperatorDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Operator.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Operator GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _OperatorDA.GetOne(payload["id"], id, request);
            return Operator.Parse(row);
        }

        public Operator Create(BaseRequest request, Dictionary<string, object> payload)
        {
            OperatorRequest operatorRequest = (request as OperatorRequest);
            if (operatorRequest == null || !operatorRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            DataRow row = _OperatorDA.Create(payload["id"], operatorRequest);
            return Operator.Parse(row);
        }

        public Operator Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            throw new InvalidOperationException("An operator cannot be updated.");
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _OperatorDA.Delete(payload["id"], id);
        }
    }
}