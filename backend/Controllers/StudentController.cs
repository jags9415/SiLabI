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
    /// Student logic.
    /// </summary>
    public class StudentController
    {
        private StudentDataAccess _StudentDA;

        /// <summary>
        /// Create a StudentController.
        /// </summary>
        public StudentController()
        {
            this._StudentDA = new StudentDataAccess();
        }

        /// <summary>
        /// Retrieves a list of students based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of students.</returns>
        public GetResponse<Student> GetStudents(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Query.Exists(element => element.Alias == "state"))
            {
                Field field = new Field("States", "Name", "state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "active"));
            }

            GetResponse<Student> response = new GetResponse<Student>();
            DataTable table = _StudentDA.GetStudents(request);
            int count = _StudentDA.GetStudentsCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Student.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get a student.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The student.</returns>
        public Student GetStudent(int id, string token)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _StudentDA.GetStudent(id);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "El identificador ingresado no corresponde a un estudiante");
            }

            return Student.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Creates a student.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The student data.</returns>
        public Student CreateStudent(StudentRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Student.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de estudiante incompletos.");
            }

            DataTable table = _StudentDA.CreateStudent(request.Student);
            return Student.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Update a student.
        /// </summary>
        /// <param name="id">The student id.</param>
        /// <param name="request">The request.</param>
        /// <returns>The student data.</returns>
        public Student UpdateStudent(int id, StudentRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Student.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de estudiante inválidos.");
            }

           DataTable table = _StudentDA.UpdateStudent(id, request.Student);
           return Student.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Delete a student.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteStudent(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _StudentDA.DeleteStudent(id);
        }
    }
}