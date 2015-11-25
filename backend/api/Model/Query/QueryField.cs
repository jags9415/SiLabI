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
    /// A field used in a query.
    /// </summary>
    public class QueryField : Field
    {
        private Relationship _relationship;
        private string _value;

        /// <summary>
        /// Creates a new QueryField
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="relationship">The relationship.</param>
        /// <param name="value">The value.</param>
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
                case SqlDbType.Bit:
                    bool boolean;
                    return Boolean.TryParse(Value, out boolean);
                case SqlDbType.Int:
                    int integer;
                    return Int32.TryParse(Value, out integer);
                case SqlDbType.Real:
                    double real;
                    return Double.TryParse(Value, out real);
                case SqlDbType.DateTime:
                    DateTimeOffset date;
                    return DateTimeOffset.TryParse(Value, out date);
                default:
                    return false;
            }
        }

        /// <summary>
        /// The relationship.
        /// </summary>
        public Relationship Relationship
        {
            get { return _relationship; }
            set { _relationship = value; }
        }

        /// <summary>
        /// The value.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            { 
                if (Type == SqlDbType.DateTime)
                {
                    var date = Converter.ToDateTimeOffset(value);
                    _value = date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                }
                else
                {
                    _value = value;
                }
            }
        }
    }
}