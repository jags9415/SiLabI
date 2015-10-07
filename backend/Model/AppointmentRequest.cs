﻿using SiLabI.Exceptions;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Contains the appointment data in a POST or PUT request to the endpoints /appointments
    /// </summary>
    [DataContract]
    public class InnerAppointmentRequest
    {
        protected string _student;
        protected string _laboratory;
        protected string _software;
        protected string _state;
        protected DateTime? _date;

        /// <summary>
        /// The student username.
        /// </summary>
        [DataMember(Name = "student")]
        public virtual string Student
        {
            set { _student = value; }
            get { return _student; }
        }

        /// <summary>
        /// The laboratory name.
        /// </summary>
        [DataMember(Name = "laboratory")]
        public virtual string Laboratory
        {
            set { _laboratory = value; }
            get { return _laboratory; }
        }

        /// <summary>
        /// The software code.
        /// </summary>
        [DataMember(Name = "software")]
        public virtual string Software
        {
            set { _software = value; }
            get { return _software; }
        }

        /// <summary>
        /// The state.
        /// </summary>
        [DataMember(Name = "state")]
        public virtual string State
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        /// The date.
        /// </summary>
        public virtual DateTime? Date
        {
            set
            { 
                if (value.HasValue && !Validator.IsValidAppointmentDate(value.Value))
                {
                    throw new InvalidParameterException("date", "Fecha inválida.");
                }
                _date = value;
            }
            get { return _date; }
        }

        /// <summary>
        /// The date in ISO-8601 format.
        /// This private field is used to parse the field in a pretty format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "date")]
        private string Date_ISO8601
        {
            get
            {
                return Date.HasValue ? Date.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") : String.Empty;
            }
            set 
            {
                Date = DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns></returns>
        public bool IsValidForCreate()
        {
            bool valid = true;

            valid &= !string.IsNullOrWhiteSpace(Software);
            valid &= Date.HasValue;

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns></returns>
        public bool IsValidForUpdate()
        {
            bool valid = true;

            if (Student != null) valid &= !string.IsNullOrWhiteSpace(Student);
            if (Laboratory != null) valid &= !string.IsNullOrWhiteSpace(Laboratory);
            if (Software != null) valid &= !string.IsNullOrWhiteSpace(Software);

            return valid;
        }
    }

    /// <summary>
    /// A POST or PUT request body to the endpoints /appointments
    /// </summary>
    [DataContract]
    public class AppointmentRequest : BaseRequest
    {
        protected InnerAppointmentRequest _appointment;

        /// <summary>
        /// The appointment data.
        /// </summary>
        [DataMember(Name = "appointment")]
        public virtual InnerAppointmentRequest Appointment
        {
            set { _appointment = value; }
            get { return _appointment; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Appointment != null;
        }
    }
}