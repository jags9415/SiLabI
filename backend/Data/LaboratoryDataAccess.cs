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

        public int GetCount(QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[0].Value = SqlUtilities.FormatWhereFields(request.Query);

            object count = _Connection.executeScalar("sp_GetLaboratoriesCount", parameters);
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

            return _Connection.executeQuery("sp_GetLaboratories", parameters);
        }

        public DataRow GetOne(int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.VarChar, id);
            parameters[1] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[1].Value = SqlUtilities.FormatSelectFields(request.Fields);
            
            DataTable table = _Connection.executeQuery("sp_GetLaboratory", parameters);
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "Laboratorio no encontrado.");
            }
            else
            {
                return table.Rows[0];
            }
        }

        public DataRow Create(object obj)
        {
            Laboratory laboratory = (obj as Laboratory);
            SqlParameter[] parameters;

            if (laboratory.Software == null)
            {
                parameters = new SqlParameter[2];
            }
            else
            {
                parameters = new SqlParameter[3];
                DataTable software = createSoftwareTable(laboratory.Software);
                parameters[2] = SqlUtilities.CreateParameter("@software", SqlDbType.Structured, software);
            }

            parameters[0] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, laboratory.Name);
            parameters[1] = SqlUtilities.CreateParameter("@seats", SqlDbType.Int, laboratory.Seats);

            DataTable table = _Connection.executeQuery("sp_CreateLaboratory", parameters);
            return table.Rows[0];
        }

        public DataRow Update(int id, object obj)
        {
            Laboratory laboratory = (obj as Laboratory);
            SqlParameter[] parameters;

            if (laboratory.Software == null)
            {
                parameters = new SqlParameter[4];
            }
            else
            {
                parameters = new SqlParameter[5];
                DataTable software = createSoftwareTable(laboratory.Software);
                parameters[4] = SqlUtilities.CreateParameter("@software", SqlDbType.Structured, software);
            }

            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
            parameters[1] = SqlUtilities.CreateParameter("@name", SqlDbType.VarChar, laboratory.Name);
            parameters[2] = SqlUtilities.CreateParameter("@seats", SqlDbType.Int, laboratory.Seats);
            parameters[3] = SqlUtilities.CreateParameter("@state", SqlDbType.VarChar, laboratory.State);

            DataTable table = _Connection.executeQuery("sp_UpdateLaboratory", parameters);
            return table.Rows[0];
        }

        public void Delete(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = SqlUtilities.CreateParameter("@id", SqlDbType.Int, id);
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