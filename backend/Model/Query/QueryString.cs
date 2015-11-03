using SiLabI.Exceptions;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace SiLabI.Model.Query
{
    /// <summary>
    /// Stores the information used in a GET request for searching, filtering, sorting and paginate.
    /// A query string have format ?access_token={token}&q={query}&sort={sort}&fields={fields}&page={page}&limit={limit}
    /// </summary>
    public class QueryString
    {
        /// <summary>
        /// Creates a new QueryString.
        /// </summary>
        /// <param name="validFields">The list of valid fields for the paratemers fields, sort, and q.</param>
        public QueryString(List<Field> validFields)
        {
            this.ValidFields = validFields;
            this.Fields = new List<Field>();
            this.Sort = new List<SortField>();
            this.Query = new List<QueryField>();
            this.Page = 1;
            this.Limit = 20;
        }

        /// <summary>
        /// Parse the page parameter.
        /// </summary>
        /// <param name="page">The page.</param>
        public void ParsePage(string page)
        {
            int result = 1;
            if ((page != null && !Int32.TryParse(page, out result)) || result <= 0)
            {
                throw new InvalidParameterException("page");
            }
            this.Page = result;
        }

        /// <summary>
        /// Parse the limit parameter.
        /// </summary>
        /// <param name="limit">The limit.</param>
        public void ParseLimit(string limit)
        {
            int result = 20;
            if ((limit != null && !Int32.TryParse(limit, out result)))
            {
                throw new InvalidParameterException("limit");
            }
            this.Limit = result;
        }

        /// <summary>
        /// Parse the fields parameter.
        /// </summary>
        /// <param name="fields">The fields.</param>
        public void ParseFields(string fields)
        {
            if (fields != null)
            {
               var result = new List<Field>();
               var splitted = fields.Split(new[] { "," }, StringSplitOptions.None);

               foreach (var item in splitted)
               {
                   Field field = Field.Find(ValidFields, item);
                   
                   if (field == null)
                   {
                       throw new InvalidParameterException("field", string.Format("[{0}]", item));
                   }

                   if (field.Type == SqlDbType.Structured)
                   {
                       result.AddRange(Field.Flatten(field.Children));
                   }
                   else
                   {
                       result.Add(field);
                   }
               }

               this.Fields = result;
            }
        }

        /// <summary>
        /// Parse the sort parameter.
        /// </summary>
        /// <param name="sort">The sort string.</param>
        public void ParseSort(string sort)
        {
            if (sort != null)
            {
                List<SortField> result = new List<SortField>();
                string[] splitted = sort.Split(new[] { "," }, StringSplitOptions.None);
                
                string name;
                SortOrder order;

                foreach (string str in splitted)
                {
                    if (str.StartsWith("-"))
                    {
                        name = str.Substring(1);
                        order = SortOrder.DESC;
                    }
                    else
                    {
                        order = SortOrder.ASC;
                        name = str;
                    }

                    Field field = Field.Find(ValidFields, name);
                    if (field == null)
                    {
                        throw new InvalidParameterException("sort", string.Format("[{0}]", str));
                    }
                    if (field.Type == SqlDbType.Structured)
                    {
                        throw new InvalidParameterException("sort", string.Format("[{0}]", "No se permiten ordenamientos mediante este campo."));
                    }
                    result.Add(new SortField(field, order));
                }

                this.Sort = result;
            }
        }

        /// <summary>
        /// Parse the query parameter.
        /// </summary>
        /// <param name="query">The query string.</param>
        public void ParseQuery(string query)
        {
            if (query != null)
            {
                List<QueryField> result = new List<QueryField>();
                string[] splitted = query.Split(new[] { "," }, StringSplitOptions.None);
                QueryField field;

                foreach (string str in splitted)
                {
                    field = ParseQueryField(str);

                    if (!field.HasValidValue())
                    {
                        throw new InvalidParameterException("q", string.Format("[{0}]", str));
                    }

                    result.Add(field);
                }

                this.Query = result;
            }
        }

        /// <summary>
        /// Parse a query field.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The query field.</returns>
        private QueryField ParseQueryField(string str)
        {
            string[] splitted = str.Split(new[] { " " }, 3, StringSplitOptions.None);

            if (splitted.Length != 3)
            {
                throw new InvalidParameterException("q", string.Format("[{0}]", str));
            }

            string key = splitted[0];
            string operation = splitted[1];
            string value = splitted[2];

            Relationship relationship;
            if (!RelationshipUtils.TryParse(operation, out relationship))
            {
                throw new InvalidParameterException("q", string.Format("Relación inválida: {0}", operation));
            }

            Field field = Field.Find(ValidFields, key);
            if (field == null)
            {
                throw new InvalidParameterException("q", string.Format("Campo inválido: {0}", key));
            }
            if (field.Type == SqlDbType.Structured)
            {
                throw new InvalidParameterException("q", string.Format("[{0}]", "No se permiten búsquedas mediante este campo."));
            }
            return new QueryField(field, relationship, value);
        }

        /// <summary>
        /// Add a new field.
        /// </summary>
        /// <param name="name">The field name.</param>
        public void AddField(string name)
        {
            Field field = Field.Find(ValidFields, name);

            if (field == null)
            {
                throw new InvalidParameterException("field", string.Format("[{0}]", name));
            }

            if (field.Type == SqlDbType.Structured)
            {
                Fields.AddRange(Field.Flatten(field.Children));
            }
            else
            {
                Fields.Add(field);
            }
        }

        /// <summary>
        /// Add a new sort field.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="order">The sort order.</param>
        public void AddSort(string name, SortOrder order)
        {
            Field field = Field.Find(ValidFields, name);

            if (field == null)
            {
                throw new InvalidParameterException("sort", string.Format("[{0}]", name));
            }

            if (field.Type == SqlDbType.Structured)
            {
                throw new InvalidParameterException("sort", string.Format("[{0}]", "No se permiten ordenamientos mediante este campo."));
            }

            Sort.Add(new SortField(field, order));
        }

        /// <summary>
        /// Add a new query field.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="relationship">The relationship.</param>
        /// <param name="value">The value.</param>
        public void AddQuery(string name, Relationship relationship, string value)
        {
            Field field = Field.Find(ValidFields, name);

            if (field == null)
            {
                throw new InvalidParameterException("q", string.Format("Campo inválido: {0}", name));
            }

            if (field.Type == SqlDbType.Structured)
            {
                throw new InvalidParameterException("q", string.Format("[{0}]", "No se permiten búsquedas mediante este campo."));
            }

            Query.Add(new QueryField(field, relationship, value));
        }

        /// <summary>
        /// The access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The number of elements per page.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// The fields to retrieve.
        /// <example>
        /// ?fields=id,code,name,period.type
        /// </example>
        /// </summary>
        public List<Field> Fields { get; set; }

        /// <summary>
        /// The fields and order for sorting the elements.
        /// </summary>
        /// <example>
        /// ?sort=+name,-age
        /// ?sort=created_at
        /// </example>
        public List<SortField> Sort { get; set; }

        /// <summary>
        /// Search elements by field.
        /// </summary>
        /// <example>
        /// ?q=name+eq+jose,created_at+ge+2015-03-01
        /// </example>
        public List<QueryField> Query { get; set; }

        /// <summary>
        /// List of valid fields.
        /// </summary>
        public List<Field> ValidFields { get; set; }
    }
}