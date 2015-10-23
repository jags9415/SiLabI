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
    /// A laboratory data.
    /// </summary>
    [DataContract]
    public class Laboratory : DatabaseObject
    {
        protected string _name;
        protected int? _seats;
        protected List<string> _software;
        protected int? _appointmentPriority;
        protected int? _reservationPriority;

        /// <summary>
        /// The laboratory name.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "name")]
        public virtual string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// The numbers of seats in the laboratory.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "seats")]
        public virtual int? Seats
        {
            set { _seats = value; }
            get { return _seats; }
        }

        /// <summary>
        /// The list of software codes that the laboratory contains.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "software")]
        public virtual List<string> Software
        {
            set { _software = value; }
            get { return _software; }
        }

        /// <summary>
        /// The priority of the laboratory for appointments.
        /// 1 = HIGH
        /// 2 = MEDIUM
        /// 3 = LOW
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "appointment_priority")]
        public virtual int? AppointmentPriority
        {
            set { _appointmentPriority = value; }
            get { return _appointmentPriority; }
        }

        /// <summary>
        /// The priority of the laboratory for reservation.
        /// 1 = HIGH
        /// 2 = MEDIUM
        /// 3 = LOW
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "reservation_priority")]
        public virtual int? ReservationPriority
        {
            set { _reservationPriority = value; }
            get { return _reservationPriority; }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns>True if the object properties are valid for a create operation.</returns>
        public override bool IsValidForCreate()
        {
            bool valid = true;

            valid &= !string.IsNullOrWhiteSpace(Name);
            valid &= Seats.HasValue && Seats >= 0;
            valid &= AppointmentPriority.HasValue && AppointmentPriority > 0;
            valid &= ReservationPriority.HasValue && ReservationPriority > 0;

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns>True if the object properties are valid for an update operation.</returns>
        public override bool IsValidForUpdate()
        {
            bool valid = true;

            if (Name != null) valid &= !string.IsNullOrWhiteSpace(Name);
            if (Seats.HasValue) valid &= Seats >= 0;
            if (AppointmentPriority.HasValue) valid &= AppointmentPriority > 0;
            if (ReservationPriority.HasValue) valid &= ReservationPriority > 0;

            return valid;
        }

        /// <summary>
        /// Fill an Laboratory object with the data provided in a DataRow.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="prefix">A string that will be prefixed to the column names of the row.</param>
        /// <returns>The Laboratory.</returns>
        public static Laboratory Parse(DataRow row, string prefix = "")
        {
            prefix = prefix.Trim();
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += ".";
            }

            Laboratory laboratory = new Laboratory();
            DatabaseObject.Parse(laboratory, row, prefix);

            laboratory.Name = row.Table.Columns.Contains(prefix + "name") ? Converter.ToString(row[prefix + "name"]) : null;
            laboratory.Seats = row.Table.Columns.Contains(prefix + "seats") ? Converter.ToNullableInt32(row[prefix + "seats"]) : null;
            laboratory.AppointmentPriority = row.Table.Columns.Contains(prefix + "appointment_priority") ? Converter.ToNullableInt32(row[prefix + "appointment_priority"]) : null;
            laboratory.ReservationPriority = row.Table.Columns.Contains(prefix + "reservation_priority") ? Converter.ToNullableInt32(row[prefix + "reservation_priority"]) : null;

            if (laboratory.isEmpty())
            {
                laboratory = null;
            }

            return laboratory;
        }
    }
}