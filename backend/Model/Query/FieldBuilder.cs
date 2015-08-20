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
        /// <param name="column">The column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The field.</returns>
        public Field VarChar(string column, string alias)
        {
            return new Field(this.Table, column, alias, SqlDbType.VarChar);
        }

        /// <summary>
        /// Creates a new Int Field.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The field.</returns>
        public Field Int(string column, string alias)
        {
            return new Field(this.Table, column, alias, SqlDbType.Int);
        }

        /// <summary>
        /// Creates a new Real Field.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The field.</returns>
        public Field Real(string column, string alias)
        {
            return new Field(this.Table, column, alias, SqlDbType.Real);
        }

        /// <summary>
        /// Creates a new DateTime Field.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns>The field.</returns>
        public Field DateTime(string column, string alias)
        {
            return new Field(this.Table, column, alias, SqlDbType.DateTime);
        }

        /// <summary>
        /// The table name.
        /// </summary>
        public string Table { get; set; }
    }
}