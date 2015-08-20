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

            return _StudentController.GetStudents(request);
        }

        public Student GetStudent(string id, string token)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return _StudentController.GetStudent(num, token);
        }

        public Student CreateStudent(StudentRequest request)
        {
            return _StudentController.CreateStudent(request);
        }

        public Student UpdateStudent(string id, StudentRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return _StudentController.UpdateStudent(num, request);
        }

        public void DeleteStudent(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidParameter", "Identificador inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            _StudentController.DeleteStudent(num, request);
        }
    }
}