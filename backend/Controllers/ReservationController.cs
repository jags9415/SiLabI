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
    /// Perform CRUD operations for Reservations.
    /// </summary>
    public class ReservationController : IController<Reservation>
    {
        private ReservationDataAccess _ReservationDA;

        /// <summary>
        /// Create a ReservationController.
        /// </summary>
        public ReservationController()
        {
            _ReservationDA = new ReservationDataAccess();
        }

        public PaginatedResponse<Reservation> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active reservations.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.Reservation, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Por iniciar"));
            }

            PaginatedResponse<Reservation> response = new PaginatedResponse<Reservation>();
            DataTable table = _ReservationDA.GetAll(payload["id"], request);
            int count = _ReservationDA.GetCount(payload["id"], request);

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(Reservation.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public Reservation GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _ReservationDA.GetOne(payload["id"], id, request);
            return Reservation.Parse(row);
        }

        public Reservation Create(BaseRequest request, Dictionary<string, object> payload)
        {
            ReservationRequest reservationRequest = (request as ReservationRequest);
            if (reservationRequest == null || !reservationRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!reservationRequest.Reservation.IsValidForCreate() || reservationRequest.Reservation.Professor == null)
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de reservación incompletos.");
            }

            DataRow row = _ReservationDA.Create(payload["id"], reservationRequest.Reservation);
            return Reservation.Parse(row);
        }

        public Reservation Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            ReservationRequest reservationRequest = (request as ReservationRequest);
            if (reservationRequest == null || !reservationRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!reservationRequest.Reservation.IsValidForUpdate())
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Datos de reservación inválidos.");
            }

            DataRow row = _ReservationDA.Update(payload["id"], id, reservationRequest.Reservation);
            return Reservation.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _ReservationDA.Delete(payload["id"], id);
        }
    }
}