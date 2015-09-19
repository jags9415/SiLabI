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
            QueryString request = new QueryString(ValidFields.Student);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _StudentController.GetAll(request);
        }

        public Student GetStudent(string username, string token, string fields)
        {
            QueryString request = new QueryString(ValidFields.Student);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _StudentController.GetOne(username, request);
        }

        public Student CreateStudent(StudentRequest request)
        {
            return _StudentController.Create(request);
        }

        public Student UpdateStudent(string id, StudentRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _StudentController.Update(num, request);
        }

        public void DeleteStudent(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _StudentController.Delete(num, request);
        }
    }
}