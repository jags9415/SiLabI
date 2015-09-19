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

        public GetResponse<Operator> GetAll(QueryString request)
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
            DataTable table = _OperatorDA.GetAll(request);
            int count = _OperatorDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Operator.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Operator GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            DataRow row = _OperatorDA.GetOne(id, request);
            return Operator.Parse(row);
        }

        public Operator Create(BaseRequest request)
        {
            OperatorRequest operatorRequest = (request as OperatorRequest);
            if (operatorRequest == null || !operatorRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(operatorRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            DataRow row = _OperatorDA.Create(operatorRequest);
            return Operator.Parse(row);
        }

        public Operator Update(int id, BaseRequest request)
        {
            throw new InvalidOperationException("An operator cannot be updated.");
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);
            _OperatorDA.Delete(id);
        }
    }
}