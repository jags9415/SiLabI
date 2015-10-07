using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A software data.
    /// </summary>
    [DataContract]
    public class Software : DatabaseObject
    {
        protected string _name;
        protected string _code;

        /// <summary>
        /// The software name.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "name")]
        public virtual string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// The software code.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "code")]
        public virtual string Code
        {
            set { _code = value; }
            get { return _code; }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns>True if the object properties are valid for a create operation.</returns>
        public override bool IsValidForCreate()
        {
            bool valid = true;

            valid &= !string.IsNullOrWhiteSpace(Name);
            valid &= !string.IsNullOrWhiteSpace(Code);

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns>True if the object properties are valid for an update operation.</returns>
        public override bool IsValidForUpdate()
        {
            bool valid = true;

            if (Name != null) valid &= !string.IsNullOrWhiteSpace(Name);
            if (Code != null) valid &= !string.IsNullOrWhiteSpace(Code);

            return valid;
        }

        /// <summary>
        /// Fill an Software object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The Software.</returns>
        public static Software Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Software software = new Software();

            software.Id = row.Table.Columns.Contains(prefix + "id") ? Converter.ToNullableInt32(row[prefix + "id"]) : null;
            software.Name = row.Table.Columns.Contains(prefix + "name") ? Converter.ToString(row[prefix + "name"]) : null;
            software.Code = row.Table.Columns.Contains(prefix + "code") ? Converter.ToString(row[prefix + "code"]) : null;
            software.CreatedAt = row.Table.Columns.Contains(prefix + "created_at") ? Converter.ToDateTime(row[prefix + "created_at"]) : null;
            software.UpdatedAt = row.Table.Columns.Contains(prefix + "updated_at") ? Converter.ToDateTime(row[prefix + "updated_at"]) : null;
            software.State = row.Table.Columns.Contains(prefix + "state") ? Converter.ToString(row[prefix + "state"]) : null;

            if (software.isEmpty())
            {
                software = null;
            }

            return software;
        }
    }
}