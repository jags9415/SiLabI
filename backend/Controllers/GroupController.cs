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
    /// Group logic.
    /// </summary>
    public class GroupController
    {
        private GroupDataAccess _GroupDA;

        /// <summary>
        /// Create a new GroupController.
        /// </summary>
        public GroupController()
        {
            _GroupDA = new GroupDataAccess();
        }

        /// <summary>
        /// Retrieves a list of groups based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of groups.</returns>
        public GetResponse<Group> GetGroups(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active groups.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Group> response = new GetResponse<Group>();
            DataTable table = _GroupDA.GetGroups(request);
            int count = _GroupDA.GetGroupsCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(ParseGroup(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The group.</returns>
        public Group GetGroup(int id, string token)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _GroupDA.GetGroup(id);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Grupo no encontrado.");
            }

            return ParseGroup(table.Rows[0]);
        }

        /// <summary>
        /// Creates a group.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The group data.</returns>
        public Group CreateGroup(GroupRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Group.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de grupo incompletos.");
            }

            DataTable table = _GroupDA.CreateGroup(request.Group);
            return ParseGroup(table.Rows[0]);
        }

        /// <summary>
        /// Update a group.
        /// </summary>
        /// <param name="id">The group id.</param>
        /// <param name="request">The request.</param>
        /// <returns>The group data.</returns>
        public Group UpdateGroup(int id, GroupRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Group.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de grupo inválidos.");
            }

            DataTable table = _GroupDA.UpdateGroup(id, request.Group);
            return ParseGroup(table.Rows[0]);
        }

        /// <summary>
        /// Delete a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteGroup(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _GroupDA.DeleteGroup(id);
        }

        /// <summary>
        /// Add a list of students to a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The request.</param>
        public void AddStudentsToGroup(int id, StudentByGroupRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _GroupDA.AddStudentsToGroup(id, request.Students);
        }

        /// <summary>
        /// Update the list of students of a group.
        /// This delete the group list and insert the given students.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The request.</param>
        public void UpdateGroupStudents(int id, StudentByGroupRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _GroupDA.UpdateGroupStudents(id, request.Students);
        }

        /// <summary>
        /// Delete a list of students from a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteStudentsFromGroup(int id, StudentByGroupRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _GroupDA.DeleteStudentsFromGroup(id, request.Students);
        }

        /// <summary>
        /// Get the list of students of a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="token">The access token</param>
        public List<Student> GetGroupStudents(int id, string token)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _GroupDA.GetGroupStudents(id);
            List<Student> students = new List<Student>();

            foreach (DataRow row in table.Rows)
            {
                students.Add(Student.Parse(row));
            }

            return students;
        }

        /// <summary>
        /// Parse a Row into A Group and fetch the Course and Professor data.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns>The group.</returns>
        private Group ParseGroup(DataRow row)
        {
            Group group = Group.Parse(row);
            group.Course = Course.Parse(row, "course");
            group.Professor = User.Parse(row, "professor");
            return group;
        }
    }
}