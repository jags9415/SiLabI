using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        public GetResponse<Student> GetStudents(string token, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            QueryString request = new QueryString(ValidFields.Student);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _StudentController.GetAll(request, payload);
        }

        public Student GetStudent(string username, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            QueryString request = new QueryString(ValidFields.Student);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _StudentController.GetOne(username, request, payload);
        }

        public Student CreateStudent(StudentRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            return _StudentController.Create(request, payload);
        }

        public Student UpdateStudent(string id, StudentRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            return _StudentController.Update(num, request, payload);
        }

        public void DeleteStudent(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _StudentController.Delete(num, request, payload);
        }
    }
}