using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class Group : BaseObject
    {
        protected int? _number;
        protected Course _course;
        protected User _professor;
        protected Period _period;

        [DataMember(EmitDefaultValue = false, Name = "number")]
        public virtual int? Number
        {
            set { _number = value; }
            get { return _number; }
        }

        [DataMember(EmitDefaultValue = false, Name = "course")]
        public virtual Course Course
        {
            set { _course = value; }
            get { return _course; }
        }

        [DataMember(EmitDefaultValue = false, Name = "professor")]
        public virtual User Professor
        {
            set { _professor = value; }
            get { return _professor; }
        }

        [DataMember(EmitDefaultValue = false, Name = "period")]
        public virtual Period Period
        {
            set { _period = value; }
            get { return _period; }
        }

        public static Group Parse(DataRow row)
        {
            Group group = new Group();

            group.Id = row.Table.Columns.Contains("id") ? Converter.ToNullableInt32(row["id"]) : null;
            group.Number = row.Table.Columns.Contains("number") ? Converter.ToNullableInt32(row["number"]) : null;
            group.CreatedAt = row.Table.Columns.Contains("created_at") ? Converter.ToDateTime(row["created_at"]) : null;
            group.UpdatedAt = row.Table.Columns.Contains("updated_at") ? Converter.ToDateTime(row["updated_at"]) : null;
            group.State = row.Table.Columns.Contains("state") ? Converter.ToString(row["state"]) : null;

            group.Period = Period.Parse(row);
            if (group.Period.Type == null && group.Period.Value == null && group.Period.Year == null)
            {
                group.Period = null;
            }

            return group;
        }
    }
}