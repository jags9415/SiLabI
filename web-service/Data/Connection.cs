using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Provides a connection to the database.
    /// </summary>
    public class Connection
    {
        private string _ConnectionString;

        /// <summary>
        /// Creates a new DBConnection.
        /// </summary>
        public Connection()
        {
            _ConnectionString = ConfigurationManager.ConnectionStrings["SiLabI"].ConnectionString;
        }

        /// <summary>
        /// Opens a new connection.
        /// </summary>
        /// <returns>The connection.</returns>
        private SqlConnection createConnection()
        {
            return new SqlConnection(_ConnectionString);
        }

        /// <summary>
        /// Execute a SELECT query and returns the data in a DataTable object.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The results.</returns>
        public DataTable executeSelectQuery(string query)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = createConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataReader reader;
                    conn.Open();
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                }
            }

            return table;
        }

        /// <summary>
        /// Execute a Stored Procedure and returns the data in a DataTable object.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The results.</returns>
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