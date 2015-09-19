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

        /// <summary>
        /// Create a new LaboratoryController.
        /// </summary>
        public LaboratoryController()
        {
            _LaboratoryDA = new LaboratoryDataAccess();
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
    }
}