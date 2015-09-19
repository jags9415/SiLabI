using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Software logic.
    /// </summary>
    public class SoftwareController
    {
        private SoftwareDataAccess _SoftwareDA;

        /// <summary>
        /// Create a CourseController.
        /// </summary>
        public SoftwareController()
        {
            this._SoftwareDA = new SoftwareDataAccess();
        }

        /// <summary>
        /// Retrieves a list of software based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of software.</returns>
        public GetResponse<Software> GetSoftwares(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active courses.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Software> response = new GetResponse<Software>();
            DataTable table = _SoftwareDA.GetSoftwares(request);
            int count = _SoftwareDA.GetSoftwareCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Software.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get a software.
        /// </summary>
        /// <param name="id">The software identification.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The software.</returns>
        public Software GetSoftware(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _SoftwareDA.GetSoftware(id, request);

            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Software no encontrado.");
            }

            return Software.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Creates a software.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The software data.</returns>
        public Software CreateSoftware(SoftwareRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Software.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de software incompletos.");
            }

            DataTable table = _SoftwareDA.CreateSoftware(request.Software);
            return Software.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Update a software.
        /// </summary>
        /// <param name="id">The software id.</param>
        /// <param name="request">The request.</param>
        /// <returns>The software data.</returns>
        public Software UpdateSoftware(int id, SoftwareRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!request.Software.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de software inválidos.");
            }

            DataTable table = _SoftwareDA.UpdateSoftware(id, request.Software);
            return Software.Parse(table.Rows[0]);
        }

        /// <summary>
        /// Delete a software.
        /// </summary>
        /// <param name="id">The software identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteSoftware(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _SoftwareDA.DeleteSoftware(id);
        }
    }
}