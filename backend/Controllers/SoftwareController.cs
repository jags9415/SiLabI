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
    /// Perform CRUD operations for Software.
    /// </summary>
    public class SoftwareController : IController<Software>
    {
        private SoftwareDataAccess _SoftwareDA;

        /// <summary>
        /// Create a CourseController.
        /// </summary>
        public SoftwareController()
        {
            this._SoftwareDA = new SoftwareDataAccess();
        }

        public GetResponse<Software> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active software.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Software, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            GetResponse<Software> response = new GetResponse<Software>();
            DataTable table = _SoftwareDA.GetAll(request);
            int count = _SoftwareDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Software.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Software GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _SoftwareDA.GetOne(id, request);
            return Software.Parse(row);
        }

        public Software GetOne(string code, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _SoftwareDA.GetOne(code, request);
            return Software.Parse(row);
        }

        public Software Create(BaseRequest request, Dictionary<string, object> payload)
        {
            SoftwareRequest softwareRequest = (request as SoftwareRequest);
            if (softwareRequest == null || !softwareRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!softwareRequest.Software.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de software incompletos.");
            }

            DataRow row = _SoftwareDA.Create(softwareRequest.Software);
            return Software.Parse(row);
        }

        public Software Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            SoftwareRequest softwareRequest = (request as SoftwareRequest);
            if (softwareRequest == null || !softwareRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!softwareRequest.Software.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de software inválidos.");
            }

            DataRow row = _SoftwareDA.Update(id, softwareRequest.Software);
            return Software.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _SoftwareDA.Delete(id);
        }
    }
}