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
    public class StudentAppointmentController : AppointmentController
    {
        /// <summary>
        /// Create a StudentAppointmentController.
        /// </summary>
        public StudentAppointmentController() : base() { }

        public GetResponse<Appointment> GetAll(string username, QueryString request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Field field;

            if (payload["type"] as string == "Estudiante")
            {
                if (username != payload["username"] as string)
                {
                    throw new UnathorizedOperationException("No se permite buscar citas de otros estudiantes");
                }

                // Block search on student field.
                if (request.Query.Exists(element => element.Parent.Name == "student"))
                {
                    throw new UnathorizedOperationException("No se permite buscar citas de otros estudiantes");
                }
            }

            // Search only the student appointments.
            field = Field.Find(ValidFields.Appointment, "student.username");
            request.Query.Add(new QueryField(field, Relationship.EQ, payload["username"] as string));

            return base.GetAll(request);
        }

        public Appointment Create(string username, BaseRequest request)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(appointmentRequest.AccessToken);
         
            if (payload["type"] as string == "Estudiante")
            {
                if (appointmentRequest.Appointment.Student == null)
                {
                    appointmentRequest.Appointment.Student = payload["username"] as string;
                }
                else if (appointmentRequest.Appointment.Student != username)
                {
                    throw new UnathorizedOperationException("No se permite crear citas para otros estudiantes");
                }
                else if (appointmentRequest.Appointment.Student != payload["username"] as string)
                {
                    throw new UnathorizedOperationException("No se permite crear citas para otros estudiantes");
                }

                if (appointmentRequest.Appointment.Laboratory != null)
                {
                    throw new UnathorizedOperationException("No se permite especificar el laboratorio de la cita");
                }
            }

            return base.Create(request);
        }

        public override Appointment Update(int id, BaseRequest request)
        {
            AppointmentRequest appointmentRequest = (request as AppointmentRequest);
            if (appointmentRequest == null || !appointmentRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            Dictionary<string, object> payload = Token.Decode(appointmentRequest.AccessToken);

            if (payload["type"] as string == "Estudiante")
            {
                if (appointmentRequest.Appointment.Student != null)
                {
                    throw new UnathorizedOperationException("No se permite cambiar el estudiante de la cita");
                }
                if (appointmentRequest.Appointment.Laboratory != null)
                {
                    throw new UnathorizedOperationException("No se permite cambiar el laboratorio de la cita");
                }
                if (appointmentRequest.Appointment.State != null)
                {
                    throw new UnathorizedOperationException("No se permite cambiar el estado de la cita");
                }
            }

            return base.Update(id, request);
        }
    }
}