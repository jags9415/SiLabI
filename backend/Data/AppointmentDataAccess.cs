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
    /// Perform CRUD operations on the Appointments table.
    /// </summary>
    public class AppointmentDataAccess : IDataAccess
    {
        private ConnectionGroup _connectionGroup;

        /// <summary>
        /// Creates a new AppointmentDataAccess.
        /// </summary>
        public AppointmentDataAccess()
        {
            _connectionGroup = ConnectionGroup.Instance;
        }

        public int GetCount(Dictionary<string, object> payload, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _connectionGroup.Get(payload["type"] as string).executeScalar("sp_GetAppointmentsCount", parameters);
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

            return _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetAppointments", parameters);
        }

        public DataRow GetOne(Dictionary<string, object> payload, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetAppointment", parameters);
            if (table.Rows.Count == 0)
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Cita no encontrada.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataTable GetAvailableForCreate(Dictionary<string, object> payload, string username, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[2] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[3] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[4] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);

            return _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetAvailableAppointmentsForCreate", parameters);
        }

        public DataTable GetAvailableForUpdate(Dictionary<string, object> payload, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);

            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[2] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[3] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[4] = SqlUtilities.CreateParameter("@appointment", SqlDbType.VarChar, id);

            return _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetAvailableAppointmentsForUpdate", parameters);
        }

        public DataRow Create(Dictionary<string, object> payload, object obj)
        {
            InnerAppointmentRequest appointment = (obj as InnerAppointmentRequest);
            SqlParameter[] parameters = new SqlParameter[6];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@student", SqlDbType.VarChar, appointment.Student);
            parameters[2] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[3] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[4] = SqlUtilities.CreateParameter("@date", SqlDbType.DateTimeOffset, appointment.Date);
            parameters[5] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, appointment.Group);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_CreateAppointment", parameters);
            return table.Rows[0];
        }

        public DataRow Update(Dictionary<string, object> payload, int id, object obj)
        {
            InnerAppointmentRequest appointment = (obj as InnerAppointmentRequest);
            SqlParameter[] parameters = new SqlParameter[9];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@student", SqlDbType.VarChar, appointment.Student);
            parameters[3] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[4] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[5] = SqlUtilities.CreateParameter("@date", SqlDbType.DateTimeOffset, appointment.Date);
            parameters[6] = SqlUtilities.CreateParameter("@attendance", SqlDbType.Bit, appointment.Attendance);
            parameters[7] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, appointment.State);
            parameters[8] = SqlUtilities.CreateParameter("@group", SqlDbType.Int, appointment.Group);

            DataTable table = _connectionGroup.Get(payload["type"] as string).executeQuery("sp_UpdateAppointment", parameters);
            return table.Rows[0];
        }

        public void Delete(Dictionary<string, object> payload, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            _connectionGroup.Get(payload["type"] as string).executeNonQuery("sp_DeleteAppointment", parameters);
        }
    }
}