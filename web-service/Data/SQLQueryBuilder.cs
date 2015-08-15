using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Data
{
    /// <summary>
    /// Build SELECTS querys over a table.
    /// </summary>
    public class SqlQueryBuilder
    {
        /// <summary>
        /// Creates a new SqlQueryBuilder.
        /// </summary>
        /// <param name="table">The table name.</param>
        public SqlQueryBuilder(string table)
        {
            this.Table = table;
            this.Joins = new List<Join>();
        }

        /// <summary>
        /// Add a join operation.
        /// </summary>
        /// <param name="type">The type of join.</param>
        /// <param name="destinationTable">The destination table.</param>
        /// <param name="sourceColumn">The source column.</param>
        /// <param name="operation">The operation between columns.</param>
        /// <param name="destinationColumn">The destination column.</param>
        public void AddJoin(JoinType type, string destinationTable, string sourceColumn, Relationship operation, string destinationColumn)
        {
            Join join = new Join();

            join.Type = type;
            join.DestinationTable = destinationTable;
            join.DestinationColumn = destinationColumn;
            join.SourceTable = this.Table;
            join.SourceColumn = sourceColumn;
            join.Relationship = operation;

            this.Joins.Add(join);
        }

        /// <summary>
        /// Generates a SQL query.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The SQL query.</returns>
        public string Query(QueryString query)
        {
            string selectedFields = GetSelectedFields(query);
            string orderByClause = GetOrderByClause(query);
            string whereClause = GetWhereClause(query);
            string joinClauses = GetJoinClauses();
            int startIndex = ((query.Page - 1) * query.Limit) + 1;
            int endIndex = query.Page * query.Limit;

            string sql = String.Format(
                "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS rn, {1} FROM {2} {3} WHERE {4}) AS SUB WHERE rn >= {5} AND rn <= {6}",
                orderByClause,
                selectedFields,
                Table,
                joinClauses,
                whereClause,
                startIndex,
                endIndex
            );

            return sql;
        }

        /// <summary>
        /// Generates the SELECT part of the SQL query.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The SELECT part of the SQL query.</returns>
        private string GetSelectedFields(QueryString query)
        {
            StringBuilder sb = new StringBuilder();
            List<Field> list;
            Field field;

            if (query.Fields.Count == 0) list = query.ValidFields;
            else list = query.Fields;

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    field = list[i];
                    sb.Append(field.Table).Append(".").Append(field.DBName).Append(" AS ").Append(field.Name);
                    if (i < list.Count - 1)
                    {
                        sb.Append(", ");
                    }
                }
            }
            else
            {
                sb.Append("*");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates the ORDER BY part of the SQL query.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The ORDER BY part of the SQL query.</returns>
        private string GetOrderByClause(QueryString query)
        {
            StringBuilder sb = new StringBuilder();

            if (query.Sort.Count > 0)
            {
                SortField field;

                for (int i = 0; i < query.Sort.Count; i++)
                {
                    field = query.Sort[i];
                    sb.Append(field.Table).Append(".").Append(field.DBName).Append(" ").Append(field.Order);
                    if (i < query.Sort.Count - 1)
                    {
                        sb.Append(", ");
                    }
                }
            }
            else
            {
                sb.Append("RAND()");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generates the WHERE part of the SQL query.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The WHERE part of the SQL query.</returns>
        private string GetWhereClause(QueryString query)
        {
            StringBuilder sb = new StringBuilder();

            if (query.Query.Count > 0)
            {
                QueryField field;

                for (int i = 0; i < query.Query.Count; i++)
                {
                    field = query.Query[i];
                    sb.Append(field.Table).Append(".").Append(field.DBName).Append(" ");
                    sb.Append(field.Operator).Append(" \'").Append(Value(field)).Append("\'");
                    if (i < query.Query.Count - 1)
                    {
                        sb.Append("AND ");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate the JOIN part of the SQL query.
        /// </summary>
        /// <returns>The JOIN part of the SQL query.</returns>
        private string GetJoinClauses()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Join join in Joins)
            {
                sb.Append(join.ToString()).Append(" ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Get the value of a WHERE field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>The value.</returns>
        private string Value(QueryField field)
        {
            if (field.Relationship == Relationship.LIKE) return field.Value.Replace("*", "%");
            else return field.Value;
        }

        /// <summary>
        /// The table name.
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// The list of joins.
        /// </summary>
        public List<Join> Joins { get; set; }
    }
}