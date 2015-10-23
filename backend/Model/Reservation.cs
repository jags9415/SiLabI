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
    /// A reservation data.
    /// </summary>
    [DataContract]
    public class Reservation : DatabaseObject
    {
        protected DateTimeOffset? _startTime;
        protected DateTimeOffset? _endTime;
        protected User _professor;
        protected Group _group;
        protected Laboratory _laboratory;
        protected Software _software;
        protected bool? _attendance;

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
        /// The group data.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "group")]
        public virtual Group Group
        {
            set { _group = value; }
            get { return _group; }
        }

        /// <summary>
        /// The attendance.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "attendance")]
        public virtual bool? Attendance
        {
            set { _attendance = value; }
            get { return _attendance; }
        }

        /// <summary>
        /// The start time.
        /// </summary>
        public virtual DateTimeOffset? StartTime
        {
            set
            {
                _startTime = value;
                StartTime_ISO8601 = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;
            }
            get { return _startTime; }
        }

        /// <summary>
        /// The end time.
        /// </summary>
        public virtual DateTimeOffset? EndTime
        {
            set
            {
                _endTime = value;
                EndTime_ISO8601 = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null;
            }
            get { return _endTime; }
        }

        /// <summary>
        /// The start time in ISO-8601 format.
        /// This private field is used to send the date in a pretty format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "start_time")]
        private string StartTime_ISO8601 { get; set; }

        /// <summary>
        /// The end time in ISO-8601 format.
        /// This private field is used to send the date in a pretty format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "end_time")]
        private string EndTime_ISO8601 { get; set; }

        /// <summary>
        /// Fill an Reservation object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The Reservation.</returns>
        public static Reservation Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Reservation reservation = new Reservation();
            DatabaseObject.Parse(reservation, row, prefix);

            reservation.StartTime = row.Table.Columns.Contains(prefix + "start_time") ? Converter.ToNullableDateTimeOffset(row[prefix + "start_time"]) : null;
            reservation.EndTime = row.Table.Columns.Contains(prefix + "end_time") ? Converter.ToNullableDateTimeOffset(row[prefix + "end_time"]) : null;
            reservation.Attendance = row.Table.Columns.Contains(prefix + "attendance") ? Converter.ToNullableBoolean(row[prefix + "attendance"]) : null;
            reservation.Professor = User.Parse(row, prefix + "professor");
            reservation.Laboratory = Laboratory.Parse(row, prefix + "laboratory");
            reservation.Software = Software.Parse(row, prefix + "software");
            reservation.Group = Group.Parse(row, prefix + "group");

            if (reservation.isEmpty())
            {
                reservation = null;
            }

            return reservation;
        }
    }
}