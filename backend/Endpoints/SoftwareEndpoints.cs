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

            return _SoftwareController.GetSoftwares(request);
        }

        public Software GetSoftware(string id, string token, string fields)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Software);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _SoftwareController.GetSoftware(num, request);
        }

        public Software CreateSoftware(SoftwareRequest request)
        {
            return _SoftwareController.CreateSoftware(request);
        }

        public Software UpdateSoftware(string id, SoftwareRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _SoftwareController.UpdateSoftware(num, request);
        }

        public void DeleteSoftware(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _SoftwareController.DeleteSoftware(num, request);
        }
    }
}