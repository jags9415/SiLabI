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
        public GetResponse<Course> GetCourses(string token, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            QueryString request = new QueryString(ValidFields.Course);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _CourseController.GetAll(request, payload);
        }

        public Course GetCourse(string id, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Course);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _CourseController.GetOne(num, request, payload);
        }

        public Course CreateCourse(CourseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            return _CourseController.Create(request, payload);
        }

        public Course UpdateCourse(string id, CourseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            return _CourseController.Update(num, request, payload);
        }

        public void DeleteCourse(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _CourseController.Delete(num, request, payload);
        }
    }
}