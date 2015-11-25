using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        public PaginatedResponse<User> GetUsers(string token, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            QueryString request = new QueryString(ValidFields.User);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _UserController.GetAll(request, payload);
        }

        public User GetUser(string username, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            QueryString request = new QueryString(ValidFields.User);
            request.AccessToken = token;
            request.ParseFields(fields);

            return _UserController.GetOne(username, request, payload);
        }
    }
}