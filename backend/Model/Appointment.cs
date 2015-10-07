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
    /// An appointment data.
    /// </summary>
    [DataContract]
    public class Appointment : DatabaseObject
    {
        protected Student _student;
        protected Laboratory _laboratory;
        protected Software _software;
        protected DateTime? _date;

        /// <summary>
        /// The student data.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "student")]
        public virtual Student Student
        {
            set { _student = value; }
            get { return _student; }
        }

        /// <summary>
        /// The laboratory data.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "laboratory")]
        public virtual Laboratory Laboratory
        {
            set { _laboratory = value; }
            get { return _laboratory; }
        }

        /// <summary>
        /// The software data.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "software")]
        public virtual Software Software
        {
            set { _software = value; }
            get { return _software; }
        }

        /// <summary>
        /// The date.
        /// </summary>
        public virtual DateTime? Date
        {
            set
            {
                _date = value;
                Date_ISO8601 = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") : null;
            }
            get { return _date; }
        }

        /// <summary>
        /// The creation date in ISO-8601 format.
        /// This private field is used to send the date in a pretty format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "date")]
        private string Date_ISO8601 { get; set; }

        /// <summary>
        /// Fill an Appointment object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The Appointment.</returns>
        public static Appointment Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Appointment appointment = new Appointment();

            appointment.Id = row.Table.Columns.Contains(prefix + "id") ? Converter.ToNullableInt32(row[prefix + "id"]) : null;
            appointment.Date = row.Table.Columns.Contains(prefix + "date") ? Converter.ToDateTime(row[prefix + "date"]) : null;
            appointment.CreatedAt = row.Table.Columns.Contains(prefix + "created_at") ? Converter.ToDateTime(row[prefix + "created_at"]) : null;
            appointment.UpdatedAt = row.Table.Columns.Contains(prefix + "updated_at") ? Converter.ToDateTime(row[prefix + "updated_at"]) : null;
            appointment.State = row.Table.Columns.Contains(prefix + "state") ? Converter.ToString(row[prefix + "state"]) : null;
            appointment.Student = Student.Parse(row, prefix + "student");
            appointment.Laboratory = Laboratory.Parse(row, prefix + "laboratory");
            appointment.Software = Software.Parse(row, prefix + "software");

            if (appointment.isEmpty())
            {
                appointment = null;
            }

            return appointment;
        }
    }
}