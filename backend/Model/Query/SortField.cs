using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Model.Query
{
    public enum SortOrder { ASC, DESC };

    /// <summary>
    /// A sort field used to order the results.
    /// For example +name, -age
    /// 
    /// Is contained in the ?sort={sort} subquery.
    /// </summary>
    public class SortField : Field
    {
        /// <summary>
        /// Create a SortField.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="order">The sort order</param>
        public SortField(Field field, SortOrder order) : base(field)
        {
            this.Order = order;
        }

        /// <summary>
        /// The order of the query.
        /// </summary>
        public SortOrder Order { get; set; }
    }
}