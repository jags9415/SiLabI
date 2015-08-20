using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Model.Query
{
    /// <summary>
    /// A field used in GET querys.
    /// A field is a property of a domain model object. 
    /// For example an user gender or a group code.
    /// 
    /// Is contained in the ?fields={fields} subquery.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="table">The table that contains this field.</param>
        /// <param name="column">The name of the field in the database.</param>
        /// <param name="alias">The alias.</param>
        public Field(string table, string column, string alias, SqlDbType type)
        {
            this.Table = table;
            this.Column = column;
            this.Alias = alias;
            this.Type = type;
        }

        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="field">The field.</param>
        public Field(Field field) : this(field.Table, field.Column, field.Alias, field.Type) { }

        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// The table that contains this field.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// The name of this field in the database.
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// The type of this field.
        /// </summary>
        public SqlDbType Type { get; set; }
    }
}