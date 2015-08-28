using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Util
{
    /// <summary>
    /// Provide utilities for SQL querys.
    /// </summary>
    public class SqlUtilities
    {
        /// <summary>
        /// Format a list of fields to SQL SELECT format.
        /// </summary>
        /// <param name="fields">The list of fields.</param>
        /// <returns>The formatted string.</returns>
        /// <example>
        /// "[name], [username], [state]"
        /// </example>
        public static string FormatSelectFields(List<Field> fields)
        {
            StringBuilder sb = new StringBuilder();
            Field field;

            for (int i = 0; i < fields.Count; i++)
            {
                field = fields[i];
                sb.AppendFormat("[{0}]", field.Name);
                if (i < fields.Count - 1) sb.Append(", ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Format a list of fields to SQL ORDER BY format.
        /// </summary>
        /// <param name="fields">The list of fields.</param>
        /// <returns>The formatted string.</returns>
        /// <example>
        /// "[gender] ASC, [phone] DESC"
        /// </example>
        public static string FormatOrderByFields(List<SortField> fields)
        {
            StringBuilder sb = new StringBuilder();
            SortField field;

            for (int i = 0; i < fields.Count; i++)
            {
                field = fields[i];
                sb.AppendFormat("[{0}]", field.Name).Append(" ").Append(field.Order);
                if (i < fields.Count - 1) sb.Append(", ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Format a list of fields to SQL WHERE format.
        /// </summary>
        /// <param name="fields">The list of fields.</param>
        /// <returns>The formatted string.</returns>
        /// <example>
        /// "[gender] = 'Male' AND [name] LIKE 'Jo%'"
        /// </example>
        public static string FormatWhereFields(List<QueryField> fields)
        {
            StringBuilder sb = new StringBuilder();
            QueryField field;

            for (int i = 0; i < fields.Count; i++)
            {
                field = fields[i];
                sb.AppendFormat("[{0}]", field.Name).Append(" ");
                sb.Append(RelationshipUtils.ToString(field.Relationship)).Append(" ");
                sb.Append("\'").Append(field.Value.Replace("*", "%")).Append("\'");
                if (i < fields.Count - 1) sb.Append(" AND ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Create a SqlParameter
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="type">The parameter type.</param>
        /// <returns>The parameter.</returns>
        public static SqlParameter CreateParameter(string name, SqlDbType type)
        {
            return new SqlParameter(name, type);
        }

        /// <summary>
        /// Create a SqlParameter
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="type">The parameter type.</param>
        /// <param name="value">The parameter value.</param>
        /// <returns>The parameter.</returns>
        public static SqlParameter CreateParameter(string name, SqlDbType type, object value)
        {
            SqlParameter parameter = CreateParameter(name, type);
            parameter.Value = (object) value ?? DBNull.Value;
            return parameter;
        }
    }
}