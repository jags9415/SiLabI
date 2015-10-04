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
    /// An reservation data.
    /// </summary>
    [DataContract]
    public class Reservation : DatabaseObject
    {
        protected DateTime? _startTime;
        protected DateTime? _endTime;
        protected User _professor;
        protected Group _group;
        protected Laboratory _laboratory;
        protected Software _software;

        /// <summary>
        /// The student.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "professor")]
        public virtual User Professor
        {
            set { _professor = value; }
            get { return _professor; }
        }

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
        /// The student.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "software")]
        public virtual Software Software
        {
            set { _software = value; }
            get { return _software; }
        }

        /// <summary>
        /// The group.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "group")]
        public virtual Group Group
        {
            set { _group = value; }
            get { return _group; }
        }

        /// <summary>
        /// The start time.
        /// </summary>
        public virtual DateTime? StartTime
        {
            set
            {
                _startTime = value;
                StartTime_ISO8601 = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") : null;
            }
            get { return _startTime; }
        }

        /// <summary>
        /// The end time.
        /// </summary>
        public virtual DateTime? EndTime
        {
            set
            {
                _endTime = value;
                EndTime_ISO8601 = value.HasValue ? value.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") : null;
            }
            get { return _endTime; }
        }

        /// <summary>
        /// The start time in ISO-8601 format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "start_time")]
        private string StartTime_ISO8601 { get; set; }

        /// <summary>
        /// The end time in ISO-8601 format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "end_time")]
        private string EndTime_ISO8601 { get; set; }

        /// <summary>
        /// Fill a Reservation object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        public static Reservation Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Reservation reservation = new Reservation();

            reservation.Id = row.Table.Columns.Contains(prefix + "id") ? Converter.ToNullableInt32(row[prefix + "id"]) : null;
            reservation.StartTime = row.Table.Columns.Contains(prefix + "start_time") ? Converter.ToDateTime(row[prefix + "start_time"]) : null;
            reservation.EndTime = row.Table.Columns.Contains(prefix + "end_time") ? Converter.ToDateTime(row[prefix + "end_time"]) : null;
            reservation.CreatedAt = row.Table.Columns.Contains(prefix + "created_at") ? Converter.ToDateTime(row[prefix + "created_at"]) : null;
            reservation.UpdatedAt = row.Table.Columns.Contains(prefix + "updated_at") ? Converter.ToDateTime(row[prefix + "updated_at"]) : null;
            reservation.State = row.Table.Columns.Contains(prefix + "state") ? Converter.ToString(row[prefix + "state"]) : null;
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