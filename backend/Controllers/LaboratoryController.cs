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
    /// Perform CRUD operations for Laboratories.
    /// </summary>
    public class LaboratoryController : IController<Laboratory>
    {
        private LaboratoryDataAccess _LaboratoryDA;
        private SoftwareByLaboratoryDataAccess _SoftwareByLaboratoryDA;

        /// <summary>
        /// Create a new LaboratoryController.
        /// </summary>
        public LaboratoryController()
        {
            _LaboratoryDA = new LaboratoryDataAccess();
            _SoftwareByLaboratoryDA = new SoftwareByLaboratoryDataAccess();
        }

        public GetResponse<Laboratory> GetAll(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            // By default search only active courses.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = new Field("state", SqlDbType.VarChar);
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Laboratory> response = new GetResponse<Laboratory>();
            DataTable table = _LaboratoryDA.GetAll(request);
            int count = _LaboratoryDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Laboratory.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Laboratory GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataRow row = _LaboratoryDA.GetOne(id, request);
            return Laboratory.Parse(row);
        }

        public Laboratory Create(BaseRequest request)
        {
            LaboratoryRequest laboratoryRequest = (request as LaboratoryRequest);
            if (laboratoryRequest == null || !laboratoryRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(laboratoryRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!laboratoryRequest.Laboratory.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de laboratorio incompletos.");
            }

            DataRow row = _LaboratoryDA.Create(laboratoryRequest.Laboratory);
            return Laboratory.Parse(row);
        }

        public Laboratory Update(int id, BaseRequest request)
        {
            LaboratoryRequest laboratoryRequest = (request as LaboratoryRequest);
            if (laboratoryRequest == null || !laboratoryRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(laboratoryRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            if (!laboratoryRequest.Laboratory.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de laboratorio inválidos.");
            }

            DataRow row = _LaboratoryDA.Update(id, laboratoryRequest.Laboratory);
            return Laboratory.Parse(row);
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _LaboratoryDA.Delete(id);
        }

        /// <summary>
        /// Get the software list of a laboratory.
        /// </summary>
        /// <param name="id">The laboratory identification.</param>
        /// <param name="request">The query.</param>
        public List<Software> GetLaboratorySoftware(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            DataTable table = _SoftwareByLaboratoryDA.GetAll(id, request);
            List<Software> software = new List<Software>();

            foreach (DataRow row in table.Rows)
            {
                software.Add(Software.Parse(row));
            }

            return software;
        }

        /// <summary>
        /// Add a software list to a laboratory.
        /// </summary>
        /// <param name="id">The laboratory identification.</param>
        /// <param name="request">The request.</param>
        public void AddSoftwareToLaboratory(int id, SoftwareByLaboratoryRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _SoftwareByLaboratoryDA.Create(id, request.Software);
        }

        /// <summary>
        /// Update the software list of a laboratory.
        /// This delete the laboratory list and insert the given software.
        /// </summary>
        /// <param name="id">The laboratory identification.</param>
        /// <param name="request">The request.</param>
        public void UpdateLaboratorySoftware(int id, SoftwareByLaboratoryRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _SoftwareByLaboratoryDA.Update(id, request.Software);
        }

        /// <summary>
        /// Delete a software list from a laboratory.
        /// </summary>
        /// <param name="id">The laboratory identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteSoftwareFromLaboratory(int id, SoftwareByLaboratoryRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);
            _SoftwareByLaboratoryDA.Delete(id, request.Software);
        }
    }
}