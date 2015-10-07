using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
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
        public PaginatedResponse<Laboratory> GetLaboratories(string token, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            QueryString request = new QueryString(ValidFields.Laboratory);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _LaboratoryController.GetAll(request, payload);
        }

        public Laboratory GetLaboratory(string id, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Laboratory);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _LaboratoryController.GetOne(num, request, payload);
        }

        public Laboratory CreateLaboratory(LaboratoryRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            return _LaboratoryController.Create(request, payload);
        }

        public Laboratory UpdateLaboratory(string id, LaboratoryRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            return _LaboratoryController.Update(num, request, payload);
        }

        public void DeleteLaboratory(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _LaboratoryController.Delete(num, request, payload);
        }

        public void AddSoftwareToLaboratory(string id, SoftwareByLaboratoryRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _LaboratoryController.AddSoftwareToLaboratory(num, request, payload);
        }

        public void UpdateLaboratorySoftware(string id, SoftwareByLaboratoryRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _LaboratoryController.UpdateLaboratorySoftware(num, request, payload);
        }

        public void DeleteSoftwareFromLaboratory(string id, SoftwareByLaboratoryRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            _LaboratoryController.DeleteSoftwareFromLaboratory(num, request, payload);
        }

        public List<Software> GetLaboratorySoftware(string id, string token, string query, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Software);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _LaboratoryController.GetLaboratorySoftware(num, request, payload);
        }
    }
}