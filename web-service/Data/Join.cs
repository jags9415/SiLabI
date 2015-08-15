using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SiLabI.Data
{
    public enum JoinType { INNER, LEFT, RIGHT }

    /// <summary>
    /// Represents a database JOIN operation.
    /// </summary>
    public class Join
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Type).Append(" JOIN ");
            sb.Append(this.DestinationTable).Append(" ON ");
            sb.Append(this.SourceTable).Append(".").Append(this.SourceColumn);
            sb.Append(" ").Append(RelationshipUtils.ToString(Relationship)).Append(" ");
            sb.Append(this.DestinationTable).Append(".").Append(this.DestinationColumn);
            return sb.ToString();
        }

        /// <summary>
        /// The source table.
        /// </summary>
        public string SourceTable { get; set; }

        /// <summary>
        /// The source column.
        /// </summary>
        public string SourceColumn { get; set; }

        /// <summary>
        /// The destination table.
        /// </summary>
        public string DestinationTable { get; set; }

        /// <summary>
        /// The destination column.
        /// </summary>
        public string DestinationColumn { get; set; }

        /// <summary>
        /// The operation between the columns.
        /// </summary>
        public Relationship Relationship { get; set; }

        /// <summary>
        /// The join type.
        /// </summary>
        public JoinType Type { get; set; }
    }
}