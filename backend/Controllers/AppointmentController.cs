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

        public GetResponse<Appointment> GetAll(QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Student);

            // By default search only active appointments.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Appointment, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Por iniciar"));
            }

            // If requester is an student, only can retrieve their own appointments.
            /*
            if (payload["type"] == "Estudiante")
            {
                if (request.Query.Exists(element => element.Parent.Name == "student"))
                {
                    throw new UnathorizedOperationException();
                }

                Field field = Field.Find(ValidFields.Appointment, "student.username");
                request.Query.Add(new QueryField(field, Relationship.EQ, payload["username"] as string));
            }
             */

            GetResponse<Appointment> response = new GetResponse<Appointment>();
            DataTable table = _AppointmentDA.GetAll(request);
            int count = _AppointmentDA.GetCount(request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Appointment.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Appointment GetOne(int id, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Student);
            DataRow row = _AppointmentDA.GetOne(id, request);
            return Appointment.Parse(row);
        }

        public Appointment Create(BaseRequest request)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(appointmentRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Student);

            if (!appointmentRequest.Appointment.IsValidForCreate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de cita incompletos.");
            }

            DataRow row = _AppointmentDA.Create(appointmentRequest.Appointment);
            return Appointment.Parse(row);
        }

        public Appointment Update(int id, BaseRequest request)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(appointmentRequest.AccessToken);
            Token.CheckPayload(payload, UserType.Student);

            if (!appointmentRequest.Appointment.IsValidForUpdate())
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Datos de cita inválidos.");
            }

            DataRow row = _AppointmentDA.Update(id, appointmentRequest.Appointment);
            return Appointment.Parse(row);
        }

        public void Delete(int id, BaseRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Student);
            _AppointmentDA.Delete(id);
        }
    }
}