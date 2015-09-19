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

        public GetResponse<User> GetAll(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active professors.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<User> response = new GetResponse<User>();
            DataTable table = _ProfessorDA.GetAll(request);
            int count = _ProfessorDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public User GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _ProfessorDA.GetOne(id, request);
            return User.Parse(row);
        }

        public User GetOne(string username, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _ProfessorDA.GetOne(username, request);
            return User.Parse(row);
        }

        public User Create(BaseRequest request)
        {
            ProfessorRequest professorRequest = (request as ProfessorRequest);
            if (professorRequest == null || !professorRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(professorRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!professorRequest.Professor.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de docente incompletos.");
            }

            DataRow row = _ProfessorDA.Create(professorRequest.Professor);
            return User.Parse(row);
        }

        public User Update(int id, BaseRequest request)
        {
            ProfessorRequest professorRequest = (request as ProfessorRequest);
            if (professorRequest == null || !professorRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(professorRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!professorRequest.Professor.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de docente inválidos.");
            }

            DataRow row = _ProfessorDA.Update(id, professorRequest.Professor);
            return User.Parse(row);
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _ProfessorDA.Delete(id);
        }
    }
}