using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A custom object field.
    /// Used for query strings in GET requests.
    /// </summary>
    public class Field
    {
        public static string[] ValidTypes = { "INT", "DOUBLE", "STRING", "DATETIME" };

        private string name;
        private string type;
        private Boolean isFixed;

        /// <summary>
        /// Creates a new Field.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="isFixed">Flag that indicates if the field is search as it is or with a wildcard.</param>
        public Field(string name, string type, bool isFixed)
        {
            this.name = name;
            this.type = type;
            this.isFixed = isFixed;

            if (!Field.ValidTypes.Contains(type))
            {
                throw new ArgumentException("Invalid type. See Field.ValidTypes for help.");
            }
        }

        /// <summary>
        /// Check if the object match the field type.
        /// </summary>
        /// <param name="obj">An object.</param>
        /// <returns>True if the object match the field type.</returns>
        public Boolean haveValidType(Object obj)
        {
            switch (type)
            {
                case "INT":
                    return obj is Int32;
                case "DOUBLE":
                    return obj is Double;
                case "STRING":
                    return obj is String;
                case "DATETIME":
                    return obj is DateTime;
                default:
                    return false;
            }
        }

        /// <summary>
        /// The name.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The type.
        /// </summary>
        public string Type
        {
            get { return Type; }
        }

        /// <summary>
        /// The fixed flag.
        /// </summary>
        public Boolean Fixed
        {
            get { return isFixed; }
        }
    }
}