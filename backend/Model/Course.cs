using SiLabI.Exceptions;
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
    /// A course data.
    /// </summary>
    [DataContract]
    public class Course : BaseObject
    {
        protected string _name;
        protected string _code;

        /// <summary>
        /// The first name.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "name")]
        public virtual string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// The first lastname.
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
        /// <returns></returns>
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
        /// <returns></returns>
        public override bool IsValidForUpdate()
        {
            bool valid = true;

            if (Name != null) valid &= !string.IsNullOrWhiteSpace(Name);
            if (Code != null) valid &= !string.IsNullOrWhiteSpace(Code);

            return valid;
        }

        /// <summary>
        /// Fill an Course object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        public static Course Parse(DataRow row)
        {
            Course course = new Course();

            course.Id = row.Table.Columns.Contains("id") ? Converter.ToNullableInt32(row["id"]) : null;
            course.Name = row.Table.Columns.Contains("name") ? Converter.ToString(row["name"]) : null;
            course.Code = row.Table.Columns.Contains("code") ? Converter.ToString(row["code"]) : null;
            course.CreatedAt = row.Table.Columns.Contains("created_at") ? Converter.ToDateTime(row["created_at"]) : null;
            course.UpdatedAt = row.Table.Columns.Contains("updated_at") ? Converter.ToDateTime(row["updated_at"]) : null;
            course.State = row.Table.Columns.Contains("state") ? Converter.ToString(row["state"]) : null;

            return course;
        }
    }
}