using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Util
{
    /// <summary>
    /// Provides utilities for casting objects.
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// Cast an object to an integer.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The integer.</returns>
        public static Int32 ToInt32(object obj)
        {
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Cast an object to an nullable integer.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The integer.</returns>
        public static Int32? ToNullableInt32(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
            {
                return null;
            }

            return Converter.ToInt32(obj);
        }

        /// <summary>
        /// Cast an object to a datetime.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The datetime.</returns>
        public static DateTime? ToDateTime(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
            {
                return null;
            }
            
            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// Cast an object to a string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The string.</returns>
        public static string ToString(object obj)
        {
            if (obj == null || Convert.IsDBNull(obj))
            {
                return null;
            }

            return Convert.ToString(obj);
        }
    }
}