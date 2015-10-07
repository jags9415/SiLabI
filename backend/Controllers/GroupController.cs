using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Perform CRUD operations for Groups.
    /// </summary>
    public class GroupController : IController<Group>
    {
        private GroupDataAccess _GroupDA;
        private StudentsByGroupDataAccess _StudentsByGroupDA;

        /// <summary>
        /// Create a new GroupController.
        /// </summary>
        public GroupController()
        {
            _GroupDA = new GroupDataAccess();
            _StudentsByGroupDA = new StudentsByGroupDataAccess();
        }

        public PaginatedResponse<Group> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active groups.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Group, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            PaginatedResponse<Group> response = new PaginatedResponse<Group>();
            DataTable table = _GroupDA.GetAll(payload["id"], request);
            int count = _GroupDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Group.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Group GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _GroupDA.GetOne(payload["id"], id, request);
            return Group.Parse(row);
        }

        public Group Create(BaseRequest request, Dictionary<string, object> payload)
        {
            GroupRequest groupRequest = (request as GroupRequest);
            if (groupRequest == null || !groupRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!groupRequest.Group.IsValidForCreate())
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de grupo incompletos.");
            }

            DataRow row = _GroupDA.Create(payload["id"], groupRequest.Group);
            return Group.Parse(row);
        }

        public Group Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            GroupRequest groupRequest = (request as GroupRequest);
            if (groupRequest == null || !groupRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!groupRequest.Group.IsValidForUpdate())
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de grupo inválidos.");
            }

            DataRow row = _GroupDA.Update(payload["id"], id, groupRequest.Group);
            return Group.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _GroupDA.Delete(payload["id"], id);
        }

        public List<Group> GetStudentGroups(string student, QueryString request, Dictionary<string, object> payload)
        {
            if (payload["type"] as string == "Estudiante" && payload["username"] as string != student)
            {
                throw new UnathorizedOperationException("No se permite buscar grupos de otros usuarios");
            }

            // By default search only active groups.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Group, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            List<Group> response = new List<Group>();
            DataTable table = _GroupDA.GetAllByStudent(payload["id"], student, request);

            foreach (DataRow row in table.Rows)
            {
                response.Add(Group.Parse(row));
            }

            return response;
        }

        /// <summary>
        /// Get the list of students of a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The query.</param>
        public List<Student> GetGroupStudents(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataTable table = _StudentsByGroupDA.GetAll(payload["id"], id, request);
            List<Student> students = new List<Student>();

            foreach (DataRow row in table.Rows)
            {
                students.Add(Student.Parse(row));
            }

            return students;
        }

        /// <summary>
        /// Add a list of students to a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The request.</param>
        public void AddStudentsToGroup(int id, StudentByGroupRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _StudentsByGroupDA.Create(payload["id"], id, request.Students);
        }

        /// <summary>
        /// Update the list of students of a group.
        /// This delete the group list and insert the given students.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The request.</param>
        public void UpdateGroupStudents(int id, StudentByGroupRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _StudentsByGroupDA.Update(payload["id"], id, request.Students);
        }

        /// <summary>
        /// Delete a list of students from a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteStudentsFromGroup(int id, StudentByGroupRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _StudentsByGroupDA.Delete(payload["id"], id, request.Students);
        }
    }
}