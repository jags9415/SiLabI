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
            SqlConnection conn = new SqlConnection(_ConnectionString);
            return conn;
        }

        /// <summary>
        /// Execute a Stored Procedure and returns the data in a DataTable object.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The results.</returns>
        public DataTable executeQuery(string name, SqlParameter[] parameters)
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

        /// <summary>
        /// Execute a Stored Procedure that doesn't return data.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        public void executeNonQuery(string name, SqlParameter[] parameters)
        {
            using (SqlConnection conn = createConnection())
            {
                using (SqlCommand cmd = new SqlCommand(name, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Execute a Stored Procedure that returns a scalar value.
        /// </summary>
        /// <param name="name">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters.</param>
        public object executeScalar(string name, SqlParameter[] parameters)
        {
            object value;

            using (SqlConnection conn = createConnection())
            {
                using (SqlCommand cmd = new SqlCommand(name, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    value = cmd.ExecuteScalar();
                }
            }

            return value;
        }
    }
}