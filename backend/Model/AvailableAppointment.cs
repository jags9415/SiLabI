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
    /// An available appointment data.
    /// </summary>
    [DataContract]
    public class AvailableAppointment : BaseObject
    {
        protected Laboratory _laboratory;
        protected DateTime? _date;
        protected int? _spaces;

        /// <summary>
        /// The laboratory.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "laboratory")]
        public virtual Laboratory Laboratory
        {
            set { _laboratory = value; }
            get { return _laboratory; }
        }

        /// <summary>
        /// The available spaces.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "spaces")]
        public virtual int? Spaces
        {
            set { _spaces = value; }
            get { return _spaces; }
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
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "date")]
        private string Date_ISO8601 { get; set; }

        /// <summary>
        /// Fill an AvailableAppointment object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        public static AvailableAppointment Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            AvailableAppointment appointment = new AvailableAppointment();

            appointment.Date = row.Table.Columns.Contains(prefix + "date") ? Converter.ToDateTime(row[prefix + "date"]) : null;
            appointment.Spaces = row.Table.Columns.Contains(prefix + "spaces") ? Converter.ToNullableInt32(row[prefix + "spaces"]) : null;
            appointment.Laboratory = Laboratory.Parse(row, prefix + "laboratory");

            if (appointment.isEmpty())
            {
                appointment = null;
            }

            return appointment;
        }
    }
}