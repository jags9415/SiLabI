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

        public GetResponse<Student> GetAll(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active students.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Student> response = new GetResponse<Student>();
            DataTable table = _StudentDA.GetAll(request);
            int count = _StudentDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Student.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Student GetOne(string username, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _StudentDA.GetOne(username, request);
            return Student.Parse(row);
        }

        public Student GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _StudentDA.GetOne(id, request);
            return Student.Parse(row);
        }

        public Student Create(BaseRequest request)
        {
            StudentRequest studentRequest = (request as StudentRequest);
            if (studentRequest == null || !studentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(studentRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!studentRequest.Student.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de estudiante incompletos.");
            }

            DataRow row = _StudentDA.Create(studentRequest.Student);
            return Student.Parse(row);
        }

        public Student Update(int id, BaseRequest request)
        {
            StudentRequest studentRequest = (request as StudentRequest);
            if (studentRequest == null || !studentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(studentRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!studentRequest.Student.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de estudiante inválidos.");
            }

            DataRow row = _StudentDA.Update(id, studentRequest.Student);
           return Student.Parse(row);
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _StudentDA.Delete(id);
        }
    }
}