using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Model.Query
{
    /// <summary>
    /// A field that indicates a query.
    /// For example name==jose age>=21
    /// 
    /// Is contained in the ?q={query} subquery.
    /// </summary>
    public class QueryField : Field
    {
        /// <summary>
        /// Creates a new QueryField
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="relationship">The query operator.</param>
        /// <param name="value">The query value.</param>
        public QueryField(Field field, Relationship relationship, string value) : base(field)
        {
            this.Relationship = relationship;
            this.Value = value;
        }

        /// <summary>
        /// Check if the value type matches the field type.
        /// </summary>
        /// <returns>True if the value type matches the field type.</returns>
        public bool HasValidValue()
        {
            switch (this.Type)
            {
                case SqlDbType.VarChar:
                    return true;
                case SqlDbType.Int:
                    int integer;
                    return Int32.TryParse(Value, out integer);
                case SqlDbType.Real:
                    double real;
                    return Double.TryParse(Value, out real);
                case SqlDbType.DateTime:
                    DateTime date;
                    return DateTime.TryParse(Value, out date);
                default:
                    return false;
            }
        }

        /// <summary>
        /// The relationship.
        /// </summary>
        public Relationship Relationship { get; set; }

        /// <summary>
        /// The query value.
        /// </summary>
        public string Value { get; set; }
    }
}