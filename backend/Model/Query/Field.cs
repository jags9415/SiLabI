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
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public Field(string name, SqlDbType type)
        {
            this.Name = name;
            this.Type = type;
        }

        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="field">The field.</param>
        public Field(Field field) : this(field.Name, field.Type) { }

        /// <summary>
        /// The name of the field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type of this field.
        /// </summary>
        public SqlDbType Type { get; set; }
    }
}