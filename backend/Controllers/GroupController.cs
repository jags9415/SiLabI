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

        public GetResponse<Group> GetAll(QueryString request)
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
            DataTable table = _GroupDA.GetAll(request);
            int count = _GroupDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(ParseGroup(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Group GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _GroupDA.GetOne(id, request);
            return ParseGroup(row);
        }

        public Group Create(BaseRequest request)
        {
            GroupRequest groupRequest = (request as GroupRequest);
            if (groupRequest == null || !groupRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(groupRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!groupRequest.Group.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de grupo incompletos.");
            }

            DataRow row = _GroupDA.Create(groupRequest.Group);
            return ParseGroup(row);
        }

        public Group Update(int id, BaseRequest request)
        {
            GroupRequest groupRequest = (request as GroupRequest);
            if (groupRequest == null || !groupRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(groupRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!groupRequest.Group.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de grupo inválidos.");
            }

            DataRow row = _GroupDA.Update(id, groupRequest.Group);
            return ParseGroup(row);
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _GroupDA.Delete(id);
        }

        /// <summary>
        /// Get the list of students of a group.
        /// </summary>
        /// <param name="id">The group identification.</param>
        /// <param name="request">The query.</param>
        public List<Student> GetGroupStudents(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _StudentsByGroupDA.GetAll(id, request);
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
        public void AddStudentsToGroup(int id, StudentByGroupRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _StudentsByGroupDA.Create(id, request.Students);
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
            _StudentsByGroupDA.Update(id, request.Students);
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
            _StudentsByGroupDA.Delete(id, request.Students);
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