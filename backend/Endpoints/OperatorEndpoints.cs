
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        public PaginatedResponse<Operator> GetOperators(string token, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Admin);
            QueryString request = new QueryString(ValidFields.Operator);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _OperatorController.GetAll(request, payload);
        }

        public Operator GetOperator(string id, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Admin);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Operator);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _OperatorController.GetOne(num, request, payload);
        }

        public Operator CreateOperator(string id, OperatorRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            request.Id = num;

            return _OperatorController.Create(request, payload);
        }

        public void DeleteOperator(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Admin);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _OperatorController.Delete(num, request, payload);
        }
    }
}