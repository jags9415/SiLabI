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
    /// Perform CRUD operations for Courses.
    /// </summary>
    public class CourseController : IController<Course>
    {
        private CourseDataAccess _CourseDA;

        /// <summary>
        /// Create a CourseController.
        /// </summary>
        public CourseController()
        {
            this._CourseDA = new CourseDataAccess();
        }

        public GetResponse<Course> GetAll(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active courses.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Course> response = new GetResponse<Course>();
            DataTable table = _CourseDA.GetAll(request);
            int count = _CourseDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Course.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Course GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _CourseDA.GetOne(id, request);
            return Course.Parse(row);
        }

        public Course Create(BaseRequest request)
        {
            CourseRequest courseRequest = (request as CourseRequest);
            if (courseRequest == null || !courseRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(courseRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!courseRequest.Course.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de curso incompletos.");
            }

            DataRow row = _CourseDA.Create(courseRequest.Course);
            return Course.Parse(row);
        }

        public Course Update(int id, BaseRequest request)
        {
            CourseRequest courseRequest = (request as CourseRequest);
            if (courseRequest == null || !courseRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(courseRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!courseRequest.Course.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de curso inválidos.");
            }

            DataRow row = _CourseDA.Update(id, courseRequest.Course);
            return Course.Parse(row);
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _CourseDA.Delete(id);
        }
    }
}