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
    /// Provide access to authentication methods on the database.
    /// </summary>
    public class AuthenticationDataAccess
    {
        private ConnectionGroup _connectionGroup;

        /// <summary>
        /// Creates a new AuthenticationDataAccess.
        /// </summary>
        public AuthenticationDataAccess()
        {
            _connectionGroup = ConnectionGroup.Instance;
        }

        /// <summary>
        /// Retrieve a user based on a credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public DataTable Authenticate(string username, string password)
        {
            SqlParameter[] parameters = new SqlParameter[2];

            parameters[0] = SqlUtilities.CreateParameter("@username", SqlDbType.VarChar, username);
            parameters[1] = SqlUtilities.CreateParameter("@password", SqlDbType.VarChar, password);

            return _connectionGroup.Get("SiLabI").executeQuery("sp_Authenticate", parameters);
        }
    }
}