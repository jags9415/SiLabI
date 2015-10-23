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
        protected DateTimeOffset? _date;
        protected int? _spaces;

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
        public virtual DateTimeOffset? Date
        {
            set
            {
                _date = value;
                Date_ISO8601 = value.HasValue ? value.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ") : null;
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
        /// Fill an AvailableAppointment object with the data provided in a DataRow.
        /// </summary>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <param name="row">The row.</param>
        /// <returns>The AvailableAppointment.</returns>
        public static AvailableAppointment Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            AvailableAppointment appointment = new AvailableAppointment();

            appointment.Date = row.Table.Columns.Contains(prefix + "date") ? Converter.ToNullableDateTimeOffset(row[prefix + "date"]) : null;
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