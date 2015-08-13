using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Stores the fields used in a GET request for searching, filtering, sorting and paginate.
    /// </summary>
    public class QueryString
    {
        private int limit;
        private int offset;
        private string accessToken;
        private List<Field> fields;
        private List<SortField> sort;
        private List<Field> validFields;
        private List<QueryField> query;

        /// <summary>
        /// Creates a new QueryString.
        /// </summary>
        /// <param name="validFields">The list of valid fields for the paratemers fields, sort, and q.</param>
        public QueryString(List<Field> validFields)
        {
            this.validFields = validFields;
        }

        /// <summary>
        /// Parse the offset parameter.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>The parse offset. 0 by default.</returns>
        private int ParseOffset(string offset)
        {
            int result = 0;
            if (offset != null && !Int32.TryParse(offset, out result))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Parámetro inválido offset=" + offset);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return result;
        }

        /// <summary>
        /// Parse the limit parameter.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns>The parsed limit. 20 by default.</returns>
        private int ParseLimit(string limit)
        {
            int result = 20;
            if (limit != null && !Int32.TryParse(limit, out result))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Parámetro inválido limit=" + limit);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return result;
        }

        /// <summary>
        /// Parse the fields parameter.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns>The parsed fields.</returns>
        private List<Field> ParseFields(string fields)
        {
            var result = new List<Field>();
            if (fields != null)
            {
               var splitted = fields.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
               foreach (var item in splitted)
               {
                   Field field = validFields.Find(element => element.Name == item);
                   if (field == null)
                   {
                       ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Parámetro inválido field=" + item);
                       throw new WebFaultException<ErrorResponse>(error, error.Code);
                   }
                   result.Add(field);
               }
            }
            return result;
        }

        /// <summary>
        /// Parse the sort parameter.
        /// </summary>
        /// <param name="sort">The sort string.</param>
        /// <returns>The parsed sort fields.</returns>
        private List<SortField> ParseSort(string sort)
        {
            List<SortField> result = new List<SortField>();
            if (sort != null)
            {
                string[] splitted = sort.Split(new[] { "," }, StringSplitOptions.None);
                string name;
                SortOrder order = SortOrder.ASC;

                foreach (string str in splitted)
                {
                    if (str.StartsWith("-"))
                    {
                        name = str.Substring(1);
                        order = SortOrder.DESC;
                    }
                    else if (str.StartsWith("+"))
                    {
                        name = str.Substring(1);
                    }
                    else
                    {
                        name = str;
                    }

                    Field field = validFields.Find(element => element.Name == name);
                    if (field == null)
                    {
                        ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Parámetro inválido sort=" + str);
                        throw new WebFaultException<ErrorResponse>(error, error.Code);
                    }
                    result.Add(new SortField(field, order));
                }
            }
            return result;
        }

        /// <summary>
        /// Parse the query parameter.
        /// </summary>
        /// <param name="query">The query string.</param>
        /// <returns>The parsed query.</returns>
        private List<QueryField> ParseQuery(string query)
        {
            List<QueryField> result = new List<QueryField>();
            if (query != null)
            {
                string[] splitted = query.Split(new[] { "," }, StringSplitOptions.None);
                QueryField field;

                foreach (string str in splitted)
                {
                    field = ParseQueryField(str);
                    result.Add(field);
                }
            }
            return result;
        }

        /// <summary>
        /// Parse a query field.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The query field.</returns>
        private QueryField ParseQueryField(string str)
        {
            QueryOperator queryOperator = QueryOperator.EQ;
            string[] row = {};

            if (str.Contains(">="))
            {
                row = str.Split(new[] { ">=" }, StringSplitOptions.None);
                queryOperator = QueryOperator.GEQ;
            }
            else if (str.Contains("<="))
            {
                row = str.Split(new[] { "<=" }, StringSplitOptions.None);
                queryOperator = QueryOperator.LEQ;
            }
            else if (str.Contains(">"))
            {
                row = str.Split(new[] { ">" }, StringSplitOptions.None);
                queryOperator = QueryOperator.GT;
            }
            else if (str.Contains("<"))
            {
                row = str.Split(new[] { "<" }, StringSplitOptions.None);
                queryOperator = QueryOperator.LT;
            }
            else if (str.Contains("!="))
            {
                row = str.Split(new[] { "!=" }, StringSplitOptions.None);
                queryOperator = QueryOperator.NEQ;
            }
            else if (str.Contains("=="))
            {
                row = str.Split(new[] { "==" }, StringSplitOptions.None);
                queryOperator = QueryOperator.EQ;
            }

            if (row.Length != 2)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Parámetro inválido q=" + str);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            string key = row[0];
            string value = row[1];

            Field field = validFields.Find(element => element.Name == key);
            if (field == null)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Parámetro inválido q=" + key);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return new QueryField(field, queryOperator, value);
        }

        /// <summary>
        /// Set the offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        public void SetOffset(string offset)
        {
            this.offset = ParseOffset(offset);
        }

        /// <summary>
        /// Set the limit.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public void SetLimit(string limit)
        {
            this.limit = ParseLimit(limit);
        }

        /// <summary>
        /// Set the fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public void SetFields(string fields)
        {
            this.fields = ParseFields(fields);
        }

        /// <summary>
        /// Set the sort.
        /// </summary>
        /// <param name="sort">The sort.</param>
        public void SetSort(string sort)
        {
            this.sort = ParseSort(sort);
        }

        /// <summary>
        /// Set the query.
        /// </summary>
        /// <param name="query">The query.</param>
        public void SetQuery(string query)
        {
            this.query = ParseQuery(query);
        }

        /// <summary>
        /// The access token.
        /// </summary>
        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }

        /// <summary>
        /// The page number.
        /// </summary>
        public int Offset
        {
            get { return offset; }
        }

        /// <summary>
        /// The number of elements per page.
        /// </summary>
        public int Limit
        {
            get { return limit; }
        }

        /// <summary>
        /// The fields to retrieve.
        /// </summary>
        public List<Field> Fields
        {
            get { return fields; }
        }

        /// <summary>
        /// The fields and order for sorting the elements.
        /// </summary>
        /// <example>
        /// ?sort=+name,-age
        /// ?sort=created_at
        /// </example>
        public List<SortField> Sort
        {
            get { return sort; }
        }

        /// <summary>
        /// Search elements by field.
        /// </summary>
        /// <example>
        /// ?q=name%3Djose%2Cgender%3Dmale
        /// </example>
        public List<QueryField> Query
        {
            get { return query; }
        }
    }
}