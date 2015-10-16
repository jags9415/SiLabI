using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
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

        public PaginatedResponse<Laboratory> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active courses.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Laboratory, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            PaginatedResponse<Laboratory> response = new PaginatedResponse<Laboratory>();
            DataTable table = _LaboratoryDA.GetAll(payload, request);
            int count = _LaboratoryDA.GetCount(payload, request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Laboratory.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Laboratory GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _LaboratoryDA.GetOne(payload, id, request);
            return Laboratory.Parse(row);
        }

        public Laboratory Create(BaseRequest request, Dictionary<string, object> payload)
        {
            LaboratoryRequest laboratoryRequest = (request as LaboratoryRequest);
            if (laboratoryRequest == null || !laboratoryRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!laboratoryRequest.Laboratory.IsValidForCreate())
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de laboratorio incompletos.");
            }

            DataRow row = _LaboratoryDA.Create(payload, laboratoryRequest.Laboratory);
            return Laboratory.Parse(row);
        }

        public Laboratory Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            LaboratoryRequest laboratoryRequest = (request as LaboratoryRequest);
            if (laboratoryRequest == null || !laboratoryRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!laboratoryRequest.Laboratory.IsValidForUpdate())
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de laboratorio inválidos.");
            }

            DataRow row = _LaboratoryDA.Update(payload, id, laboratoryRequest.Laboratory);
            return Laboratory.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _LaboratoryDA.Delete(payload, id);
        }

        /// <summary>
        /// Get the software list of a laboratory.
        /// </summary>
        /// <param name="id">The laboratory identification.</param>
        /// <param name="request">The query.</param>
        public List<Software> GetLaboratorySoftware(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataTable table = _SoftwareByLaboratoryDA.GetAll(payload, id, request);
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
        public void AddSoftwareToLaboratory(int id, SoftwareByLaboratoryRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _SoftwareByLaboratoryDA.Create(payload, id, request.Software);
        }

        /// <summary>
        /// Update the software list of a laboratory.
        /// This delete the laboratory list and insert the given software.
        /// </summary>
        /// <param name="id">The laboratory identification.</param>
        /// <param name="request">The request.</param>
        public void UpdateLaboratorySoftware(int id, SoftwareByLaboratoryRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _SoftwareByLaboratoryDA.Update(payload, id, request.Software);
        }

        /// <summary>
        /// Delete a software list from a laboratory.
        /// </summary>
        /// <param name="id">The laboratory identification.</param>
        /// <param name="request">The request.</param>
        public void DeleteSoftwareFromLaboratory(int id, SoftwareByLaboratoryRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _SoftwareByLaboratoryDA.Delete(payload, id, request.Software);
        }
    }
}