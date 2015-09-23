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
        public GetResponse<Appointment> GetAppointments(string token, string query, string page, string limit, string sort, string fields)
        {
            QueryString request = new QueryString(ValidFields.Appointment);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _AppointmentController.GetAll(request);
        }

        public Appointment GetAppointment(string id, string token, string fields)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Appointment);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _AppointmentController.GetOne(num, request);
        }

        public Appointment CreateAppointment(AppointmentRequest request)
        {
            return _AppointmentController.Create(request);
        }

        public Appointment UpdateAppointment(string id, AppointmentRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _AppointmentController.Update(num, request);
        }

        public void DeleteAppointment(string id, BaseRequest request)
        {
            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _AppointmentController.Delete(num, request);
        }
    }
}