using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SiLabI.Model.Query
{
    /// <summary>
    /// Build fields for an defined table.
    /// </summary>
    public class FieldBuilder
    {
        /// <summary>
        /// Creates a new FieldBuilder.
        /// </summary>
        /// <param name="table">The table name.</param>
        public FieldBuilder(string table)
        {
            this.Table = table;
        }

        /// <summary>
        /// Creates a new VarChar Field.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="dbName">The column name.</param>
        /// <returns>The field.</returns>
        public Field VarChar(string name, string dbName)
        {
            return new Field(this.Table, name, dbName, SqlDbType.VarChar);
        }

        /// <summary>
        /// Creates a new Int Field.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="dbName">The column name.</param>
        /// <returns>The field.</returns>
        public Field Int(string name, string dbName)
        {
            return new Field(this.Table, name, dbName, SqlDbType.Int);
        }

        /// <summary>
        /// Creates a new Real Field.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="dbName">The column name.</param>
        /// <returns>The field.</returns>
        public Field Real(string name, string dbName)
        {
            return new Field(this.Table, name, dbName, SqlDbType.Real);
        }

        /// <summary>
        /// Creates a new DateTime Field.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="dbName">The column name.</param>
        /// <returns>The field.</returns>
        public Field DateTime(string name, string dbName)
        {
            return new Field(this.Table, name, dbName, SqlDbType.DateTime);
        }

        /// <summary>
        /// The table name.
        /// </summary>
        public string Table { get; set; }
    }
}