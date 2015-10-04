using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
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
        private Connection _Connection;

        /// <summary>
        /// Creates a new ReservationDataAccess.
        /// </summary>
        public ReservationDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(object requesterId, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetReservationsCount", parameters);
            return Converter.ToInt32(count);
        }

        public DataTable GetAll(object requesterId, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[6];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);

            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[2] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[3] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[4] = SqlUtilities.CreateParameter("@page", SqlDbType.Int, request.Page);
            parameters[5] = SqlUtilities.CreateParameter("@limit", SqlDbType.Int, request.Limit);

            return _Connection.executeQuery("sp_GetReservations", parameters);
        }

        public DataRow GetOne(object requesterId, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetReservation", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Reservación no encontrada.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object requesterId, object obj)
        {
            InnerReservationRequest appointment = (obj as InnerReservationRequest);
            SqlParameter[] parameters = new SqlParameter[7];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, appointment.Professor);
            parameters[2] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[3] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[4] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, appointment.Group);
            parameters[5] = SqlUtilities.CreateParameter("@start_time", SqlDbType.DateTime, appointment.StartTime);
            parameters[6] = SqlUtilities.CreateParameter("@end_time", SqlDbType.DateTime, appointment.EndTime);

            DataTable table = _Connection.executeQuery("sp_CreateReservation", parameters);
            return table.Rows[0];
        }

        public DataRow Update(object requesterId, int id, object obj)
        {
            InnerReservationRequest appointment = (obj as InnerReservationRequest);
            SqlParameter[] parameters = new SqlParameter[9];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@professor", SqlDbType.VarChar, appointment.Professor);
            parameters[3] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[4] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[5] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, appointment.Group);
            parameters[6] = SqlUtilities.CreateParameter("@start_time", SqlDbType.DateTime, appointment.StartTime);
            parameters[7] = SqlUtilities.CreateParameter("@end_time", SqlDbType.DateTime, appointment.EndTime);
            parameters[8] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, appointment.State);

            DataTable table = _Connection.executeQuery("sp_UpdateReservation", parameters);
            return table.Rows[0];
        }

        public void Delete(object requesterId, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            _Connection.executeNonQuery("sp_DeleteReservation", parameters);
        }
    }
}