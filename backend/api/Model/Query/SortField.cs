using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Model.Query
{
    /// <summary>
    /// Define a sorting order.
    /// </summary>
    public enum SortOrder { ASC, DESC };

    /// <summary>
    /// A sort field used to order the results.
    /// </summary>
    public class SortField : Field
    {
        /// <summary>
        /// Create a SortField.
        /// </summary>
        /// <param name="name">The field.</param>
        /// <param name="order">The sorting order.</param>
        public SortField(Field field, SortOrder order) : base(field)
        {
            this.Order = order;
        }

        /// <summary>
        /// The sorting order.
        /// </summary>
        public SortOrder Order { get; set; }
    }
}