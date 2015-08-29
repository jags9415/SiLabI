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
    public class Operator : Student
    {
        protected Period _period;

        /// <summary>
        /// Creates a empty operator.
        /// </summary>
        public Operator() : base()
        {
            this.Period = new Period();
        }

        /// <summary>
        /// Clones an user.
        /// </summary>
        /// <param name="user">The user to clone.</param>
        public Operator(Student user) : base(user)
        {
            this.Period = new Period();
        }

        /// <summary>
        /// The operator period.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "period")]
        public Period Period
        {
            get { return _period; }
            set { _period = value; }
        }

        /// <summary>
        /// Fill an Operator object with the data provided in a DataRow.
        /// </summary>
        /// <param name="user">The user.</param>
        public static Operator Parse(DataRow row)
        {
            Operator op = new Operator(Student.Parse(row));

            op.Period.Type = row.Table.Columns.Contains("period.type") ? Converter.ToString(row["period.type"]) : null;
            op.Period.Value = row.Table.Columns.Contains("period.value") ? Converter.ToNullableInt32(row["period.value"]) : null;
            op.Period.Year = row.Table.Columns.Contains("period.year") ? Converter.ToNullableInt32(row["period.year"]) : null;

            if (op.Period.Type == null && op.Period.Value == null && op.Period.Year == null)
            {
                op.Period = null;
            }

            return op;
        }
    }
}