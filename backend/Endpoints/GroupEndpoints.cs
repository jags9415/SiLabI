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
        public GetResponse<Group> GetGroups(string token, string query, string page, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(ValidFields.Group);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _GroupController.GetGroups(request);
        }

        public Group GetGroup(string id, string token)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _GroupController.GetGroup(num, token);
        }

        public Group CreateGroup(GroupRequest request)
        {
            return _GroupController.CreateGroup(request);
        }

        public Group UpdateGroup(string id, GroupRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _GroupController.UpdateGroup(num, request);
        }

        public void DeleteGroup(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _GroupController.DeleteGroup(num, request);
        }

        public void AddStudentsToGroup(string id, StudentByGroupRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _GroupController.AddStudentsToGroup(num, request);
        }

        public void UpdateGroupStudents(string id, StudentByGroupRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _GroupController.UpdateGroupStudents(num, request);
        }

        public void DeleteStudentsFromGroup(string id, StudentByGroupRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _GroupController.DeleteStudentsFromGroup(num, request);
        }

        public List<Student> GetGroupStudents(string id, string token)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _GroupController.GetGroupStudents(num, token);
        }
    }
}