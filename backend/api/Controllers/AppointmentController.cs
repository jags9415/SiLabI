using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
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

        /// <summary>
        /// Get all the appointments that satisfies a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>A GetResponse containing the appointments list and the pagination information.</returns>
        public PaginatedResponse<Appointment> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active appointments.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Appointment, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Por iniciar"));
            }
  
            PaginatedResponse<Appointment> response = new PaginatedResponse<Appointment>();
            DataTable table = _AppointmentDA.GetAll(payload, request);
            int count = _AppointmentDA.GetCount(payload, request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Appointment.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        /// <summary>
        /// Get the available appointment dates for a specific student.
        /// </summary>
        /// <param name="student">The student username.</param>
        /// <param name="request">The query.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The list of available appointments dates of the student.</returns>
        public List<AvailableAppointment> GetAvailableForCreate(string student, QueryString request, Dictionary<string, object> payload)
        {
            if (payload["type"] as string == "Estudiante" && payload["username"] as string != student)
            {
                throw new UnathorizedOperationException("No se permite buscar citas de otros usuarios");
            }

            List<AvailableAppointment> response = new List<AvailableAppointment>();
            DataTable table = _AppointmentDA.GetAvailableForCreate(payload, student, request);

            foreach (DataRow row in table.Rows)
            {
                response.Add(AvailableAppointment.Parse(row));
            }

            return response;
        }

        /// <summary>
        /// Get the available update dates for a specific appointment.
        /// </summary>
        /// <param name="student">The student username.</param>
        /// <param name="id">The appointment identity</param>
        /// <param name="request">The query.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The list of available appointments update dates.</returns>
        public List<AvailableAppointment> GetAvailableForUpdate(string student, int id, QueryString request, Dictionary<string, object> payload)
        {
            if (payload["type"] as string == "Estudiante" && payload["username"] as string != student)
            {
                throw new UnathorizedOperationException("No se permite buscar citas de otros usuarios");
            }

            List<AvailableAppointment> response = new List<AvailableAppointment>();
            DataTable table = _AppointmentDA.GetAvailableForUpdate(payload, id, request);

            foreach (DataRow row in table.Rows)
            {
                response.Add(AvailableAppointment.Parse(row));
            }

            return response;
        }

        /// <summary>
        /// Get a specific appointment.
        /// </summary>
        /// <param name="id">The appointment identity.</param>
        /// <param name="request">The query.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The appointment.</returns>
        public Appointment GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _AppointmentDA.GetOne(payload, id, request);
            return Appointment.Parse(row);
        }

        /// <summary>
        /// Create an appointment.
        /// </summary>
        /// <param name="request">The create request.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The created appointment.</returns>
        public Appointment Create(BaseRequest request, Dictionary<string, object> payload)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!appointmentRequest.Appointment.IsValidForCreate() || appointmentRequest.Appointment.Student == null)
            {
                throw new BaseException(HttpStatusCode.BadRequest, "Datos de cita incompletos.");
            }

            DataRow row = _AppointmentDA.Create(payload, appointmentRequest.Appointment);
            return Appointment.Parse(row);
        }

        /// <summary>
        /// Update an appointment.
        /// </summary>
        /// <param name="id">The appointment identity.</param>
        /// <param name="request">The update request.</param>
        /// <param name="payload">The token payload.</param>
        /// <returns>The updated appointment.</returns>
        public Appointment Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!appointmentRequest.Appointment.IsValidForUpdate())
            {
                throw new BaseException(HttpStatusCode.BadRequest, "Datos de cita inválidos.");
            }

            DataRow row = _AppointmentDA.Update(payload, id, appointmentRequest.Appointment);
            return Appointment.Parse(row);
        }

        /// <summary>
        /// Delete an appointment.
        /// </summary>
        /// <param name="id">The appointment identity.</param>
        /// <param name="request">The delete request.</param>
        /// <param name="payload">The token payload.</param>
        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _AppointmentDA.Delete(payload, id);
        }
    }
}