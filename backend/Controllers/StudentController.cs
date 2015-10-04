using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Perform CRUD operations for Students.
    /// </summary>
    public class StudentController : IController<Student>
    {
        private StudentDataAccess _StudentDA;

        /// <summary>
        /// Create a StudentController.
        /// </summary>
        public StudentController()
        {
            this._StudentDA = new StudentDataAccess();
        }

        public GetResponse<Student> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active students.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Student, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Student> response = new GetResponse<Student>();
            DataTable table = _StudentDA.GetAll(payload["id"], request);
            int count = _StudentDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Student.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Student GetOne(string username, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _StudentDA.GetOne(payload["id"], username, request);
            return Student.Parse(row);
        }

        public Student GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _StudentDA.GetOne(payload["id"], id, request);
            return Student.Parse(row);
        }

        public Student Create(BaseRequest request, Dictionary<string, object> payload)
        {
            StudentRequest studentRequest = (request as StudentRequest);
            if (studentRequest == null || !studentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!studentRequest.Student.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de estudiante incompletos.");
            }

            DataRow row = _StudentDA.Create(payload["id"], studentRequest.Student);
            return Student.Parse(row);
        }

        public Student Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            StudentRequest studentRequest = (request as StudentRequest);
            if (studentRequest == null || !studentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!studentRequest.Student.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de estudiante inválidos.");
            }

            DataRow row = _StudentDA.Update(payload["id"], id, studentRequest.Student);
           return Student.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _StudentDA.Delete(payload["id"], id);
        }
    }
}