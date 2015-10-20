using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Perform CRUD operations on the Reservations table.
    /// </summary>
    public class ReservationDataAccess : IDataAccess
    {
        private ConnectionGroup _connectionGroup;

        /// <summary>
        /// Creates a new ReservationDataAccess.
        /// </summary>
        public ReservationDataAccess()
        {
            _connectionGroup = ConnectionGroup.Instance;
        }

        public int GetCount(Dictionary<string, object> payload, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _connectionGroup.Get(payload["type"] as string).executeScalar("sp_GetReservationsCount", parameters);
            return Converter.ToInt32(count);
        }

        public DataTable GetAll(Dictionary<string, object> payload, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[6];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[2] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[3] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[4] = SqlUtilities.CreateParameter("@page", SqlDbType.Int, request.Page);
            parameters[5] = SqlUtilities.CreateParameter("@limit", SqlDbType.Int, request.Limit);

            return _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetReservations", parameters);
        }

        public DataRow GetOne(Dictionary<string, object> payload, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetReservation", parameters);
            if (table.Rows.Count == 0)
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Reservación no encontrada.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(Dictionary<string, object> payload, object obj)
        {
            InnerReservationRequest appointment = (obj as InnerReservationRequest);
            SqlParameter[] parameters = new SqlParameter[7];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, appointment.Professor);
            parameters[2] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[3] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[4] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, appointment.Group);
            parameters[5] = SqlUtilities.CreateParameter("@start_time", SqlDbType.DateTime, appointment.StartTime);
            parameters[6] = SqlUtilities.CreateParameter("@end_time", SqlDbType.DateTime, appointment.EndTime);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_CreateReservation", parameters);
            return table.Rows[0];
        }

        public DataRow Update(Dictionary<string, object> payload, int id, object obj)
        {
            InnerReservationRequest appointment = (obj as InnerReservationRequest);
            SqlParameter[] parameters = new SqlParameter[10];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, appointment.Professor);
            parameters[3] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[4] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[5] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, appointment.Group);
            parameters[6] = SqlUtilities.CreateParameter("@start_time", SqlDbType.DateTime, appointment.StartTime);
            parameters[7] = SqlUtilities.CreateParameter("@end_time", SqlDbType.DateTime, appointment.EndTime);
            parameters[8] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, appointment.State);
            parameters[9] = SqlUtilities.CreateParameter("@attendance", SqlDbType.Bit, appointment.Attendance);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_UpdateReservation", parameters);
            return table.Rows[0];
        }

        public void Delete(Dictionary<string, object> payload, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            _connectionGroup.Get(payload["type"] as string).executeNonQuery("sp_DeleteReservation", parameters);
        }
    }
}