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
    /// Perform CRUD operations on the Appointments table.
    /// </summary>
    public class AppointmentDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new AppointmentDataAccess.
        /// </summary>
        public AppointmentDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetAppointmentsCount", parameters);
            return Converter.ToInt32(count);
        }

        public DataTable GetAll(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[1] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[2] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatWhereFields(request.Query);

            parameters[3] = SqlUtilities.CreateParameter("@page", SqlDbType.Int, request.Page);
            parameters[4] = SqlUtilities.CreateParameter("@limit", SqlDbType.Int, request.Limit);

            return _Connection.executeQuery("sp_GetAppointments", parameters);
        }

        public DataRow GetOne(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);

            DataTable table = _Connection.executeQuery("sp_GetAppointment", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Cita no encontrada.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object obj)
        {
            InnerAppointmentRequest appointment = (obj as InnerAppointmentRequest);
            SqlParameter[] parameters = new SqlParameter[4];

            parameters[0] = SqlUtilities.CreateParameter("@student", SqlDbType.VarChar, appointment.Student);
            parameters[1] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[2] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[3] = SqlUtilities.CreateParameter("@date", SqlDbType.DateTime, appointment.Date);

            DataTable table = _Connection.executeQuery("sp_CreateAppointment", parameters);
            return table.Rows[0];
        }

        public DataRow Update(int id, object obj)
        {
            InnerAppointmentRequest appointment = (obj as InnerAppointmentRequest);
            SqlParameter[] parameters = new SqlParameter[6];

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@student", SqlDbType.VarChar, appointment.Student);
            parameters[2] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.VarChar, appointment.Laboratory);
            parameters[3] = SqlUtilities.CreateParameter("@software", SqlDbType.VarChar, appointment.Software);
            parameters[4] = SqlUtilities.CreateParameter("@date", SqlDbType.DateTime, appointment.Date);
            parameters[5] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, appointment.State);

            DataTable table = _Connection.executeQuery("sp_UpdateAppointment", parameters);
            return table.Rows[0];
        }

        public void Delete(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            _Connection.executeNonQuery("sp_DeleteAppointment", parameters);
        }
    }
}