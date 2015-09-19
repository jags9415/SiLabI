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
    /// Professor logic.
    /// </summary>
    public class ProfessorController
    {
        private ProfessorDataAccess _ProfessorDA;

        /// <summary>
        /// Create a ProfessorController.
        /// </summary>
        public ProfessorController()
        {
            this._ProfessorDA = new ProfessorDataAccess();
        }

        /// <summary>
        /// Retrieves a list of professors based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of professors.</returns>
        public GetResponse<User> GetProfessors(QueryString request)
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
            DataTable table = _ProfessorDA.GetProfessors(request);
            int count = _ProfessorDA.GetProfessorsCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get a professor.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The professor.</returns>
        public User GetProfessor(string username, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _ProfessorDA.GetProfessor(username, request);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Docente no encontrado.");
            }

            return User.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Creates a professor.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The professor data.</returns>
        public User CreateProfessor(ProfessorRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Professor.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de docente incompletos.");
            }

            DataTable table = _ProfessorDA.CreateProfessor(request.Professor);
            return User.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Update a professor.
        /// </summary>
        /// <param name="id">The professor id.</param>
        /// <param name="request">The request.</param>
        /// <returns>The professor data.</returns>
        public User UpdateProfessor(int id, ProfessorRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Professor.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de docente inválidos.");
            }

            DataTable table = _ProfessorDA.UpdateProfessor(id, request.Professor);
            return User.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Delete a professor.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteProfessor(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _ProfessorDA.DeleteProfessor(id);
        }
    }
}