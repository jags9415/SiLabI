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
    /// Perform CRUD operations on the Laboratories table.
    /// </summary>
    public class LaboratoryDataAccess : IDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Create a new LaboratoryDataAccess.
        /// </summary>
        public LaboratoryDataAccess()
        {
            _Connection = new Connection();
        }

        public int GetCount(object requesterId, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);

            parameters[1] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetLaboratoriesCount", parameters);
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

            return _Connection.executeQuery("sp_GetLaboratories", parameters);
        }

        public DataRow GetOne(object requesterId, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[3];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.VarChar, id);
            
            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);
            
            DataTable table = _Connection.executeQuery("sp_GetLaboratory", parameters);
            if (table.Rows.Count == 0)
            {
                throw new SiLabIException(HttpStatusCode.BadRequest, "Laboratorio no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object requesterId, object obj)
        {
            Laboratory laboratory = (obj as Laboratory);
            SqlParameter[] parameters;

            if (laboratory.Software == null)
            {
                parameters = new SqlParameter[5];
            }
            else
            {
                parameters = new SqlParameter[6];
                DataTable software = createSoftwareTable(laboratory.Software);
                parameters[5] = SqlUtilities.CreateParameter("@software", SqlDbType.Structured, software);
            }

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, laboratory.Name);
            parameters[2] = SqlUtilities.CreateParameter("@seats", SqlDbType.Int, laboratory.Seats);
            parameters[3] = SqlUtilities.CreateParameter("@appointment_priority", SqlDbType.Int, laboratory.AppointmentPriority);
            parameters[4] = SqlUtilities.CreateParameter("@reservation_priority", SqlDbType.Int, laboratory.ReservationPriority);

            DataTable table = _Connection.executeQuery("sp_CreateLaboratory", parameters);
            return table.Rows[0];
        }

        public DataRow Update(object requesterId, int id, object obj)
        {
            Laboratory laboratory = (obj as Laboratory);
            SqlParameter[] parameters;

            if (laboratory.Software == null)
            {
                parameters = new SqlParameter[7];
            }
            else
            {
                parameters = new SqlParameter[8];
                DataTable software = createSoftwareTable(laboratory.Software);
                parameters[7] = SqlUtilities.CreateParameter("@software", SqlDbType.Structured, software);
            }

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, laboratory.Name);
            parameters[3] = SqlUtilities.CreateParameter("@seats", SqlDbType.Int, laboratory.Seats);
            parameters[4] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, laboratory.State);
            parameters[5] = SqlUtilities.CreateParameter("@appointment_priority", SqlDbType.Int, laboratory.AppointmentPriority);
            parameters[6] = SqlUtilities.CreateParameter("@reservation_priority", SqlDbType.Int, laboratory.ReservationPriority);

            DataTable table = _Connection.executeQuery("sp_UpdateLaboratory", parameters);
            return table.Rows[0];
        }

        public void Delete(object requesterId, int id)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, requesterId);
            parameters[1] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);

            _Connection.executeNonQuery("sp_DeleteLaboratory", parameters);
        }

        private DataTable createSoftwareTable(List<string> codes)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Code");
            codes.ForEach(x => table.Rows.Add(x));
            return table;
        }
    }
}