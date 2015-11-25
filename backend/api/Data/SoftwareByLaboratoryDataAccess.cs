using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// perform CRUD operations on the SoftwareByLaboratory table.
    /// </summary>
    public class SoftwareByLaboratoryDataAccess : IDataAccessIntermediate
    {
        private ConnectionGroup _connectionGroup;

        /// <summary>
        /// Creates a new SoftwareByLaboratoryDataAccess.
        /// </summary>
        public SoftwareByLaboratoryDataAccess()
        {
            _connectionGroup = ConnectionGroup.Instance;
        }

        public DataTable GetAll(Dictionary<string, object> payload, int id, QueryString request)
        {
            SqlParameter[] parameters = new SqlParameter[5];

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.Int, id);

            parameters[2] = SqlUtilities.CreateParameter("@fields", SqlDbType.VarChar);
            parameters[2].Value = SqlUtilities.FormatSelectFields(request.Fields);

            parameters[3] = SqlUtilities.CreateParameter("@order_by", SqlDbType.VarChar);
            parameters[3].Value = SqlUtilities.FormatOrderByFields(request.Sort);

            parameters[4] = SqlUtilities.CreateParameter("@where", SqlDbType.VarChar);
            parameters[4].Value = SqlUtilities.FormatWhereFields(request.Query);

            return _connectionGroup.Get(payload["type"] as string).executeQuery("sp_GetSoftwareByLaboratory", parameters);
        }

        public void Create(Dictionary<string, object> payload, int id, object obj)
        {
            List<string> students = (obj as List<string>);
            SqlParameter[] parameters = new SqlParameter[3];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@softwares", SqlDbType.Structured, table);

            _connectionGroup.Get(payload["type"] as string).executeNonQuery("sp_AddSoftwareToLaboratory", parameters);
        }

        public void Update(Dictionary<string, object> payload, int id, object obj)
        {
            List<string> students = (obj as List<string>);
            SqlParameter[] parameters = new SqlParameter[3];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@softwares", SqlDbType.Structured, table);

            _connectionGroup.Get(payload["type"] as string).executeNonQuery("sp_UpdateLaboratorySoftware", parameters);
        }

        public void Delete(Dictionary<string, object> payload, int id, object obj)
        {
            List<string> students = (obj as List<string>);
            SqlParameter[] parameters = new SqlParameter[3];

            DataTable table = new DataTable();
            table.Columns.Add("Username");
            students.ForEach(x => table.Rows.Add(x));

            parameters[0] = SqlUtilities.CreateParameter("@requester_id", SqlDbType.Int, payload["id"]);
            parameters[1] = SqlUtilities.CreateParameter("@laboratory", SqlDbType.Int, id);
            parameters[2] = SqlUtilities.CreateParameter("@softwares", SqlDbType.Structured, table);

            _connectionGroup.Get(payload["type"] as string).executeNonQuery("sp_RemoveSoftwareFromLaboratory", parameters);
        }
    }
}