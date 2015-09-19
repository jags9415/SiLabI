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
        public GetResponse<Laboratory> GetLaboratories(string token, string query, string page, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(ValidFields.Laboratory);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _LaboratoryController.GetAll(request);
        }

        public Laboratory GetLaboratory(string id, string token, string fields)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Laboratory);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _LaboratoryController.GetOne(num, request);
        }

        public Laboratory CreateLaboratory(LaboratoryRequest request)
        {
            return _LaboratoryController.Create(request);
        }

        public Laboratory UpdateLaboratory(string id, LaboratoryRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _LaboratoryController.Update(num, request);
        }

        public void DeleteLaboratory(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _LaboratoryController.Delete(num, request);
        }
    }
}