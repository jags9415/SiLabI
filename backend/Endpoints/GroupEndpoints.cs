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
        public PaginatedResponse<Group> GetGroups(string token, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            QueryString request = new QueryString(ValidFields.Group);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _GroupController.GetAll(request, payload);
        }

        public Group GetGroup(string id, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Group);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _GroupController.GetOne(num, request, payload);
        }

        public Group CreateGroup(GroupRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            return _GroupController.Create(request, payload);
        }

        public Group UpdateGroup(string id, GroupRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            return _GroupController.Update(num, request, payload);
        }

        public void DeleteGroup(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _GroupController.Delete(num, request, payload);
        }

        public void AddStudentsToGroup(string id, StudentByGroupRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _GroupController.AddStudentsToGroup(num, request, payload);
        }

        public void UpdateGroupStudents(string id, StudentByGroupRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _GroupController.UpdateGroupStudents(num, request, payload);
        }

        public void DeleteStudentsFromGroup(string id, StudentByGroupRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _GroupController.DeleteStudentsFromGroup(num, request, payload);
        }

        public List<Student> GetGroupStudents(string id, string token, string query, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Student);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _GroupController.GetGroupStudents(num, request, payload);
        }

        public List<Group> GetStudentGroups(string student, string token, string query, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Student);

            QueryString request = new QueryString(ValidFields.Group);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _GroupController.GetStudentGroups(student, request, payload);
        }
    }
}