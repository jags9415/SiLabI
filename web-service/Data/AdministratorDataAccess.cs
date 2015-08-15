using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Provide access to the data related to Administrators.
    /// </summary>
    public class AdministratorDataAccess
    {
        private Connection _Connection;
        private SqlQueryBuilder _SqlBuilder;
        private FieldBuilder _FieldBuilder;

        /// <summary>
        /// Creates a new AdministratorDataAccess.
        /// </summary>
        public AdministratorDataAccess()
        {
            _Connection = new Connection();
            _FieldBuilder = new FieldBuilder("States");
            _SqlBuilder = new SqlQueryBuilder("Administrators");
            _SqlBuilder.AddJoin(JoinType.INNER, "Users", "FK_User_Id", Relationship.EQ, "PK_User_Id");
            _SqlBuilder.AddJoin(JoinType.INNER, "States", "FK_State_Id", Relationship.EQ, "PK_State_Id");
        }

        /// <summary>
        /// Get all the administrators that satisfies the query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>A DataTable that contains all the administrators data that satisfies the query.</returns>
        public DataTable GetAdministrators(QueryString request)
        {
            if (request.Query.Find(element => element.Name == "state") == null)
            {
                request.Query.Add(new QueryField(_FieldBuilder.VarChar("state", "Name"), Relationship.EQ, "active"));
            }

            String query = _SqlBuilder.Query(request);
            return _Connection.executeSelectQuery(query);
        }

        /// <summary>
        /// Get a specific administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <returns>A DataTable that contains the administrator data.</returns>
        public DataTable GetAdministrator(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = new SqlParameter("@user_id", SqlDbType.Int);
            parameters[0].Value = id;

            return _Connection.executeStoredProcedure("sp_GetAdministrator", parameters);
        }

        /// <summary>
        /// Creates an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        public void CreateAdministrator(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = new SqlParameter("@user_id", SqlDbType.Int);
            parameters[0].Value = id;

            _Connection.executeStoredProcedure("sp_CreateAdministrator", parameters);
        }

        /// <summary>
        /// Deletes an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        public void DeleteAdministrator(int id)
        {
            SqlParameter[] parameters = new SqlParameter[1];

            parameters[0] = new SqlParameter("@user_id", SqlDbType.Int);
            parameters[0].Value = id;

            _Connection.executeStoredProcedure("sp_DeleteAdministrator", parameters);
        }
    }
}