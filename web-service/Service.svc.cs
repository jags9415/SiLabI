using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using System.IO;
using System.Reflection;
using System.ServiceModel.Activation;
using SiLabI.Model;
using SiLabI.Controllers;
using System.Net;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service : IService
    {
        private AuthenticationController authController;
        private StudentController studentController;

        public Service()
        {
            this.authController = new AuthenticationController();
            this.studentController = new StudentController();
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            return authController.Authenticate(request);
        }

        public List<Student> GetStudents(string token, string query, string offset, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(Student.ValidFields);
            request.AccessToken = token;
            request.SetQuery(query);
            request.SetOffset(offset);
            request.SetLimit(limit);
            request.SetSort(sort);
            request.SetFields(fields);
            return studentController.GetStudents(request);
        }

        public Student GetStudent(string id, string token)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "ID inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return studentController.GetStudent(num, token);
        }

        public Student CreateStudent(StudentRequest request)
        {
            return studentController.CreateStudent(request);
        }

        public Student UpdateStudent(string id, StudentRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "ID inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return studentController.UpdateStudent(num, request);
        }

        public void DeleteStudent(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "ID inválido: " + id);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            studentController.DeleteStudent(num, request);
        }
    }
}
