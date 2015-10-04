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
    /// Perform CRUD operations for Professors.
    /// </summary>
    public class ProfessorController : IController<User>
    {
        private ProfessorDataAccess _ProfessorDA;

        /// <summary>
        /// Create a ProfessorController.
        /// </summary>
        public ProfessorController()
        {
            this._ProfessorDA = new ProfessorDataAccess();
        }

        public GetResponse<User> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active professors.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Professor, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<User> response = new GetResponse<User>();
            DataTable table = _ProfessorDA.GetAll(payload["id"], request);
            int count = _ProfessorDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public User GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _ProfessorDA.GetOne(payload["id"], id, request);
            return User.Parse(row);
        }

        public User GetOne(string username, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _ProfessorDA.GetOne(payload["id"], username, request);
            return User.Parse(row);
        }

        public User Create(BaseRequest request, Dictionary<string, object> payload)
        {
            ProfessorRequest professorRequest = (request as ProfessorRequest);
            if (professorRequest == null || !professorRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!professorRequest.Professor.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de docente incompletos.");
            }

            DataRow row = _ProfessorDA.Create(payload["id"], professorRequest.Professor);
            return User.Parse(row);
        }

        public User Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            ProfessorRequest professorRequest = (request as ProfessorRequest);
            if (professorRequest == null || !professorRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!professorRequest.Professor.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de docente inválidos.");
            }

            DataRow row = _ProfessorDA.Update(payload["id"], id, professorRequest.Professor);
            return User.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _ProfessorDA.Delete(payload["id"], id);
        }
    }
}