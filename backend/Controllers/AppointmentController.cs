using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Perform CRUD operations for Appointments.
    /// </summary>
    public class AppointmentController : IController<Appointment>
    {
        private AppointmentDataAccess _AppointmentDA;

        /// <summary>
        /// Create a AppointmentController.
        /// </summary>
        public AppointmentController()
        {
            _AppointmentDA = new AppointmentDataAccess();
        }

        public virtual GetResponse<Appointment> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active appointments.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Appointment, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Por iniciar"));
            }
  
            GetResponse<Appointment> response = new GetResponse<Appointment>();
            DataTable table = _AppointmentDA.GetAll(payload["id"], request);
            int count = _AppointmentDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Appointment.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public virtual List<AvailableAppointment> GetAvailable(string username, QueryString request, Dictionary<string, object> payload)
        {
            if (payload["type"] as string == "Estudiante" && payload["username"] as string != username)
            {
                throw new UnathorizedOperationException("No se permite buscar citas de otros usuarios");
            }

            List<AvailableAppointment> response = new List<AvailableAppointment>();
            DataTable table = _AppointmentDA.GetAvailable(payload["id"], username, request);

            foreach (DataRow row in table.Rows)
            {
                response.Add(AvailableAppointment.Parse(row));
            }

            return response;
        }

        public virtual Appointment GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _AppointmentDA.GetOne(payload["id"], id, request);
            return Appointment.Parse(row);
        }

        public virtual Appointment Create(BaseRequest request, Dictionary<string, object> payload)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!appointmentRequest.Appointment.IsValidForCreate() || appointmentRequest.Appointment.Student == null)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de cita incompletos.");
            }

            DataRow row = _AppointmentDA.Create(payload["id"], appointmentRequest.Appointment);
            return Appointment.Parse(row);
        }

        public virtual Appointment Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!appointmentRequest.Appointment.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de cita inválidos.");
            }

            DataRow row = _AppointmentDA.Update(payload["id"], id, appointmentRequest.Appointment);
            return Appointment.Parse(row);
        }

        public virtual void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _AppointmentDA.Delete(payload["id"], id);
        }
    }
}