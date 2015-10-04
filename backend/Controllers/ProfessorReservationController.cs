using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Controllers
{
    public class ProfessorReservationController
    {
        private ReservationController _reservationController;

        /// <summary>
        /// Create a ProfessorReservationController.
        /// </summary>
        public ProfessorReservationController()
        {
            _reservationController = new ReservationController();
        }

        public GetResponse<Reservation> GetAll(string username, QueryString request, Dictionary<string, object> payload)
        {
            Field field;

            if (payload["type"] as string == "Docente")
            {
                if (username != payload["username"] as string)
                {
                    throw new UnathorizedOperationException("No se permite buscar reservaciones de otros docentes");
                }

                // Block search on professor field.
                if (request.Query.Exists(element => element.Parent.Name == "professor"))
                {
                    throw new UnathorizedOperationException("No se permite buscar reservaciones de otros docentes");
                }
            }

            // Search only the professor reservations.
            field = Field.Find(ValidFields.Reservation, "professor.username");
            request.Query.Add(new QueryField(field, Relationship.EQ, payload["username"] as string));

            return _reservationController.GetAll(request, payload);
        }

        public Reservation Create(string username, BaseRequest request, Dictionary<string, object> payload)
        {
            ReservationRequest reservationRequest = (request as ReservationRequest);
            if (reservationRequest == null || !reservationRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }
         
            if (payload["type"] as string == "Docente")
            {
                if (reservationRequest.Reservation.Professor == null)
                {
                    reservationRequest.Reservation.Professor = payload["username"] as string;
                }
                else if (reservationRequest.Reservation.Professor != username)
                {
                    throw new UnathorizedOperationException("No se permite crear reservaciones para otros docentes");
                }
                else if (reservationRequest.Reservation.Professor != payload["username"] as string)
                {
                    throw new UnathorizedOperationException("No se permite crear reservaciones para otros docentes");
                }
            }

            return _reservationController.Create(request, payload);
        }

        public Reservation Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            ReservationRequest reservationRequest = (request as ReservationRequest);
            if (reservationRequest == null || !reservationRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (payload["type"] as string == "Docente")
            {
                if (reservationRequest.Reservation.Professor != null)
                {
                    throw new UnathorizedOperationException("No se permite cambiar el docente de la reservación");
                }
                if (reservationRequest.Reservation.State != null)
                {
                    throw new UnathorizedOperationException("No se permite cambiar el estado de la reservación");
                }
            }

            return _reservationController.Update(id, request, payload);
        }
    }
}