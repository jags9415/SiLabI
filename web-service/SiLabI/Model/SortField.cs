using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Model
{
    public enum SortOrder { ASC, DESC };

    /// <summary>
    /// A sort query.
    /// </summary>
    public class SortField : Field
    {
        private SortOrder order;

        /// <summary>
        /// Create a SortField.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="order">The sort order</param>
        public SortField(Field field, SortOrder order) : base(field.Name, field.Type, field.Fixed)
        {
            this.order = order;
        }

        /// <summary>
        /// The order of the query.
        /// </summary>
        /// <example>
        /// ASC
        /// DESC
        /// </example>
        public SortOrder Order
        {
            get { return order; }
        }
    }
}