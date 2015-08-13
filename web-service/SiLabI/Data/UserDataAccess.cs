using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    public class UserDataAccess
    {
        private DBConnection conn;

        /// <summary>
        /// Creates a new UserDataAccess.
        /// </summary>
        public UserDataAccess()
        {
            conn = new DBConnection();
        }

        /// <summary>
        /// Retrieve a user based on a credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public DataTable authenticate(string username, string password)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = new SqlParameter("@username", SqlDbType.VarChar);
            parameters[0].Value = username;
            parameters[1] = new SqlParameter("@password", SqlDbType.VarChar);
            parameters[1].Value = password;

            DataTable table = conn.executeStoredProcedure("sp_Authenticate", parameters);
            return table;
        }
    }
}