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
    /// A laboratory data.
    /// </summary>
    [DataContract]
    public class Laboratory : DatabaseObject
    {
        protected string _name;
        protected int? _seats;
        protected List<string> _software;

        /// <summary>
        /// The name.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "name")]
        public virtual string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// The code.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "seats")]
        public virtual int? Seats
        {
            set { _seats = value; }
            get { return _seats; }
        }

        /// <summary>
        /// The software list.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "software")]
        public virtual List<string> Software
        {
            set { _software = value; }
            get { return _software; }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns></returns>
        public override bool IsValidForCreate()
        {
            bool valid = true;

            valid &= !string.IsNullOrWhiteSpace(Name);
            valid &= Seats.HasValue && Seats >= 0;

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns></returns>
        public override bool IsValidForUpdate()
        {
            bool valid = true;

            if (Name != null) valid &= !string.IsNullOrWhiteSpace(Name);
            if (Seats.HasValue) valid &= Seats >= 0;

            return valid;
        }

        /// <summary>
        /// Fill an Laboratory object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        public static Laboratory Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Laboratory laboratory = new Laboratory();

            laboratory.Id = row.Table.Columns.Contains(prefix + "id") ? Converter.ToNullableInt32(row[prefix + "id"]) : null;
            laboratory.Name = row.Table.Columns.Contains(prefix + "name") ? Converter.ToString(row[prefix + "name"]) : null;
            laboratory.Seats = row.Table.Columns.Contains(prefix + "seats") ? Converter.ToNullableInt32(row[prefix + "seats"]) : null;
            laboratory.CreatedAt = row.Table.Columns.Contains(prefix + "created_at") ? Converter.ToDateTime(row[prefix + "created_at"]) : null;
            laboratory.UpdatedAt = row.Table.Columns.Contains(prefix + "updated_at") ? Converter.ToDateTime(row[prefix + "updated_at"]) : null;
            laboratory.State = row.Table.Columns.Contains(prefix + "state") ? Converter.ToString(row[prefix + "state"]) : null;

            if (laboratory.isEmpty())
            {
                laboratory = null;
            }

            return laboratory;
        }
    }
}