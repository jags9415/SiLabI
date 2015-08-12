using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Model
{
    public enum QueryOperator { EQ, NEQ, GEQ, LEQ, GT, LT }

    /// <summary>
    /// A query.
    /// </summary>
    public class QueryField : Field
    {
        private QueryOperator queryOperator;
        private Object value;

        /// <summary>
        /// Creates a new QueryField
        /// </summary>
        /// <param name="field">The field</param>
        /// <param name="queryOperator">The operation</param>
        /// <param name="value">The value</param>
        public QueryField(Field field, QueryOperator queryOperator, Object value) : base(field.Name, field.Type, field.Fixed)
        {
            this.queryOperator = queryOperator;
            this.value = value;
        }

        /// <summary>
        /// The query operator.
        /// </summary>
        public QueryOperator Operator
        {
            get { return queryOperator; }
        }

        /// <summary>
        /// The query value.
        /// </summary>
        public Object Value
        {
            get { return value; }
        }
    }
}