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
            QueryString request = new QueryString(ValidFields.Course);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _CourseController.GetCourses(request);
        }

        public Course GetCourse(string id, string token, string fields)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Course);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _CourseController.GetCourse(num, request);
        }

        public Course CreateCourse(CourseRequest request)
        {
            return _CourseController.CreateCourse(request);
        }

        public Course UpdateCourse(string id, CourseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _CourseController.UpdateCourse(num, request);
        }

        public void DeleteCourse(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _CourseController.DeleteCourse(num, request);
        }
    }
}