using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// A collection of database connections.
    /// </summary>
    public class ConnectionGroup
    {
        private ConnectionStringSettingsCollection _connectionStrings;
        private static ConnectionGroup _instance;

        /// <summary>
        /// Create a new ConnectionGroup.
        /// </summary>
        private ConnectionGroup()
        {
            _connectionStrings = ConfigurationManager.ConnectionStrings;
        }

        /// <summary>
        /// Get a connection.
        /// </summary>
        /// <param name="name">The connection name.</param>
        /// <returns>The connection.</returns>
        public Connection Get(string name)
        {
            return new Connection(_connectionStrings[name]);
        }

        /// <summary>
        /// The singleton instance.
        /// </summary>
        public static ConnectionGroup Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConnectionGroup();
                }
                return _instance;
            }
        }
    }
}