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
        public GetResponse<Software> GetSoftwares(string token, string query, string page, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(ValidFields.Software);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _SoftwareController.GetAll(request);
        }

        public Software GetSoftware(string code, string token, string fields)
        {
            QueryString request = new QueryString(ValidFields.Software);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _SoftwareController.GetOne(code, request);
        }

        public Software CreateSoftware(SoftwareRequest request)
        {
            return _SoftwareController.Create(request);
        }

        public Software UpdateSoftware(string id, SoftwareRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _SoftwareController.Update(num, request);
        }

        public void DeleteSoftware(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _SoftwareController.Delete(num, request);
        }
    }
}