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
    /// A group data.
    /// </summary>
    [DataContract]
    public class Group : DatabaseObject
    {
        protected int? _number;
        protected Course _course;
        protected User _professor;
        protected Period _period;

        /// <summary>
        /// The group number.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "number")]
        public virtual int? Number
        {
            set { _number = value; }
            get { return _number; }
        }

        /// <summary>
        /// The course data.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "course")]
        public virtual Course Course
        {
            set { _course = value; }
            get { return _course; }
        }

        /// <summary>
        /// The professor data.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "professor")]
        public virtual User Professor
        {
            set { _professor = value; }
            get { return _professor; }
        }

        /// <summary>
        /// The period.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "period")]
        public virtual Period Period
        {
            set { _period = value; }
            get { return _period; }
        }

        /// <summary>
        /// Fill an Group object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The Group.</returns>
        public static Group Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Group group = new Group();
            DatabaseObject.Parse(group, row, prefix);

            group.Number = row.Table.Columns.Contains(prefix + "number") ? Converter.ToNullableInt32(row[prefix + "number"]) : null;
            group.Period = Period.Parse(row, prefix + "period");
            group.Course = Course.Parse(row, prefix + "course");
            group.Professor = User.Parse(row, prefix + "professor");

            if (group.isEmpty())
            {
                group = null;
            }

            return group;
        }
    }
}