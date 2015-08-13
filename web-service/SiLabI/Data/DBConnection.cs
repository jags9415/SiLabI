using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    public class DBConnection
    {
        private string connectionString;

        /// <summary>
        /// Creates a new DBConnection.
        /// </summary>
        public DBConnection()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SiLabI"].ConnectionString;
        }

        /// <summary>
        /// Opens a new connection.
        /// </summary>
        /// <returns>The connection.</returns>
        private SqlConnection createConnection()
        {
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Execute a stored procedure with rows as result.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The rows returned by the procedure</returns>
        public DataTable executeStoredProcedure(string name, SqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            
            using (SqlConnection conn = createConnection())
            {
                using (SqlCommand cmd = new SqlCommand(name, conn))
                {
                    SqlDataReader reader;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                }
            }

            return table;
        }
    }
}