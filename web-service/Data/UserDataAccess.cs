using SiLabI.Model;
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
    /// Provides the access to the data related to Users.
    /// </summary>
    public class UserDataAccess
    {
        private Connection _Connection;

        /// <summary>
        /// Creates a new UserDataAccess.
        /// </summary>
        public UserDataAccess()
        {
            _Connection = new Connection();
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

            parameters[0] = new SqlParameter("@username", SqlDbType.VarChar);
            parameters[0].Value = username;
            parameters[1] = new SqlParameter("@password", SqlDbType.VarChar);
            parameters[1].Value = password;

            DataTable table = _Connection.executeStoredProcedure("sp_Authenticate", parameters);
            return table;
        }

        /// <summary>
        /// Fill an User object with the data provided in a DataRow.
        /// </summary>
        /// <param name="user">The user.</param>
        public User ParseUser(DataRow row)
        {
            User user = new User();

            user.Id = row.Table.Columns.Contains("id") ? Converter.ToInt32(row["id"]) : 0;
            user.Name = row.Table.Columns.Contains("name") ? Converter.ToString(row["name"]) : null;
            user.LastName1 = row.Table.Columns.Contains("last_name_1") ? Converter.ToString(row["last_name_1"]) : null;
            user.LastName2 = row.Table.Columns.Contains("last_name_2") ? Converter.ToString(row["last_name_2"]) : null;
            user.Gender = row.Table.Columns.Contains("gender") ? Converter.ToString(row["gender"]) : null;
            user.Email = row.Table.Columns.Contains("email") ? Converter.ToString(row["email"]) : null;
            user.Phone = row.Table.Columns.Contains("phone") ? Converter.ToString(row["phone"]) : null;
            user.Username = row.Table.Columns.Contains("username") ? Converter.ToString(row["username"]) : null;
            user.CreatedAt = row.Table.Columns.Contains("created_at") ? Converter.ToDateTime(row["created_at"]) : null;
            user.UpdatedAt = row.Table.Columns.Contains("updated_at") ? Converter.ToDateTime(row["updated_at"]) : null;
            user.State = row.Table.Columns.Contains("state") ? Converter.ToString(row["state"]) : null;

            return user;
        }
    }
}