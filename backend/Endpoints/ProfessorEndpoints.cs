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
        public GetResponse<User> GetProfessors(string token, string query, string page, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(ValidFields.Student);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _ProfessorController.GetAll(request);
        }

        public User GetProfessor(string username, string token, string fields)
        {
            QueryString request = new QueryString(ValidFields.Professor);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _ProfessorController.GetOne(username, request);
        }

        public User CreateProfessor(ProfessorRequest request)
        {
            return _ProfessorController.Create(request);
        }

        public User UpdateProfessor(string id, ProfessorRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _ProfessorController.Update(num, request);
        }

        public void DeleteProfessor(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _ProfessorController.Delete(num, request);
        }
    }
}