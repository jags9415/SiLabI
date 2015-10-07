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

        public PaginatedResponse<Course> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active courses.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Course, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            PaginatedResponse<Course> response = new PaginatedResponse<Course>();
            DataTable table = _CourseDA.GetAll(payload["id"], request);
            int count = _CourseDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Course.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Course GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _CourseDA.GetOne(payload["id"], id, request);
            return Course.Parse(row);
        }

        public Course Create(BaseRequest request, Dictionary<string, object> payload)
        {
            CourseRequest courseRequest = (request as CourseRequest);
            if (courseRequest == null || !courseRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!courseRequest.Course.IsValidForCreate())
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de curso incompletos.");
            }

            DataRow row = _CourseDA.Create(payload["id"], courseRequest.Course);
            return Course.Parse(row);
        }

        public Course Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            CourseRequest courseRequest = (request as CourseRequest);
            if (courseRequest == null || !courseRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!courseRequest.Course.IsValidForUpdate())
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de curso inválidos.");
            }

            DataRow row = _CourseDA.Update(payload["id"], id, courseRequest.Course);
            return Course.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _CourseDA.Delete(payload["id"], id);
        }
    }
}