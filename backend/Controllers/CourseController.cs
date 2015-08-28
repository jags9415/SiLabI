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
    /// Course logic.
    /// </summary>
    public class CourseController
    {
        private CourseDataAccess _CourseDA;

        /// <summary>
        /// Create a CourseController.
        /// </summary>
        public CourseController()
        {
            this._CourseDA = new CourseDataAccess();
        }

        /// <summary>
        /// Retrieves a list of courses based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of courses.</returns>
        public GetResponse<Course> GetCourses(QueryString request)
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
            DataTable table = _CourseDA.GetCourses(request);
            int count = _CourseDA.GetCourseCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Course.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get a course.
        /// </summary>
        /// <param name="id">The course identification.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The course.</returns>
        public Course GetCourse(int id, string token)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _CourseDA.GetCourse(id);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Curso no encontrado.");
            }

            return Course.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Creates a course.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The course data.</returns>
        public Course CreateCourse(CourseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Course.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de curso incompletos.");
            }

            DataTable table = _CourseDA.CreateCourse(request.Course);
            return Course.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Update a course.
        /// </summary>
        /// <param name="id">The course id.</param>
        /// <param name="request">The request.</param>
        /// <returns>The course data.</returns>
        public Course UpdateCourse(int id, CourseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Course.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de curso inválidos.");
            }

            DataTable table = _CourseDA.UpdateCourse(id, request.Course);
            return Course.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Delete a course.
        /// </summary>
        /// <param name="id">The course identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteCourse(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _CourseDA.DeleteCourse(id);
        }
    }
}