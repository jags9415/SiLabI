using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Controllers
{
    public class StudentAppointmentController
    {
        private AppointmentController _appointmentController;

        /// <summary>
        /// Create a StudentAppointmentController.
        /// </summary>
        public StudentAppointmentController()
        {
            _appointmentController = new AppointmentController();
        }

        public PaginatedResponse<Appointment> GetAll(string username, QueryString request, Dictionary<string, object> payload)
        {
            Field field;

            if (username != payload["username"] as string)
            {
                throw new UnathorizedOperationException("No se permite buscar citas de otros usuarios");
            }

            // Block search on student field.
            if (request.Query.Exists(element => element.Parent != null && element.Parent.Name == "student"))
            {
                throw new UnathorizedOperationException("No se permite buscar citas de otros usuarios");
            }

            // Search only the student appointments.
            field = Field.Find(ValidFields.Appointment, "student.username");
            request.Query.Add(new QueryField(field, Relationship.EQ, payload["username"] as string));

            return _appointmentController.GetAll(request, payload);
        }

        public Appointment Create(string username, BaseRequest request, Dictionary<string, object> payload)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }
         
            if (appointmentRequest.Appointment.Student != null && appointmentRequest.Appointment.Student != username)
            {
                throw new UnathorizedOperationException("No se permite crear citas para otros usuarios");
            }
            if (username != payload["username"] as string)
            {
                throw new UnathorizedOperationException("No se permite crear citas para otros usuarios");
            }
            if (appointmentRequest.Appointment.Laboratory != null)
            {
                throw new UnathorizedOperationException("No se permite especificar el laboratorio de la cita");
            }

            appointmentRequest.Appointment.Student = username;
            return _appointmentController.Create(request, payload);
        }

        public Appointment Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (appointmentRequest.Appointment.Student != null)
            {
                throw new UnathorizedOperationException("No se permite cambiar el usuario de la cita");
            }
            if (appointmentRequest.Appointment.Laboratory != null)
            {
                throw new UnathorizedOperationException("No se permite cambiar el laboratorio de la cita");
            }
            if (appointmentRequest.Appointment.State != null)
            {
                throw new UnathorizedOperationException("No se permite cambiar el estado de la cita");
            }

            return _appointmentController.Update(id, request, payload);
        }
    }
}