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
        public Operator() : base() { }

        /// <summary>
        /// Clones an user.
        /// </summary>
        /// <param name="user">The user to clone.</param>
        public Operator(Student user) : base(user) { }

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
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The Operator.</returns>
        public static Operator Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Operator @operator = new Operator(Student.Parse(row, prefix));
            @operator.Period = Period.Parse(row, prefix + "period");

            if (@operator.isEmpty())
            {
                @operator = null;
            }

            return @operator;
        }
    }
}