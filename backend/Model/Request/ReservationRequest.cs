using SiLabI.Exceptions;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model.Request
{
    [DataContract]
    public class InnerReservationRequest
    {
        protected string _professor;
        protected string _laboratory;
        protected string _software;
        protected string _state;
        protected int? _group;
        protected DateTime? _startTime;
        protected DateTime? _endTime;

        /// <summary>
        /// The professor username.
        /// </summary>
        [DataMember(Name = "professor")]
        public virtual string Professor
        {
            set { _professor = value; }
            get { return _professor; }
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
        /// The group identity.
        /// </summary>
        [DataMember(Name = "group")]
        public virtual int? Group
        {
            set { _group = value; }
            get { return _group; }
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
        /// The start time.
        /// </summary>
        public virtual DateTime? StartTime
        {
            set
            {
                if (value.HasValue && !Validator.IsValidReservationDate(value.Value))
                {
                    throw new InvalidParameterException("start_time", "Ingrese una dia posterior a hoy, hora entre 8:00 - 17:00");
                }
                _startTime = value;
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
                if (value.HasValue && !Validator.IsValidReservationDate(value.Value))
                {
                    throw new InvalidParameterException("end_time", "Ingrese una dia posterior a hoy, hora entre 8:00 - 18:00");
                }
                _endTime = value;
            }
            get { return _endTime; }
        }

        /// <summary>
        /// The start time in ISO-8601 format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "start_time")]
        private string StartTime_ISO8601
        {
            get
            {
                return StartTime.HasValue ? StartTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") : String.Empty;
            }
            set
            {
                StartTime = DateTime.Parse(value);
            }
        }

        /// <summary>
        /// The end time in ISO-8601 format.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "end_time")]
        private string EndTime_ISO8601
        {
            get
            {
                return EndTime.HasValue ? EndTime.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff") : String.Empty;
            }
            set
            {
                EndTime = DateTime.Parse(value);
            }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns></returns>
        public bool IsValidForCreate()
        {
            bool valid = true;

            valid &= StartTime.HasValue;
            valid &= EndTime.HasValue;

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns></returns>
        public bool IsValidForUpdate()
        {
            bool valid = true;

            if (Professor != null) valid &= !string.IsNullOrWhiteSpace(Professor);
            if (Laboratory != null) valid &= !string.IsNullOrWhiteSpace(Laboratory);
            if (State != null) valid &= !string.IsNullOrWhiteSpace(State);
            if (Group != null) valid &= Group >= 0;

            return valid;
        }
    }

    [DataContract]
    public class ReservationRequest : BaseRequest
    {
        protected InnerReservationRequest _reservation;

        /// <summary>
        /// The reservation data.
        /// </summary>
        [DataMember(Name = "reservation")]
        public virtual InnerReservationRequest Reservation
        {
            set { _reservation = value; }
            get { return _reservation; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid() && Reservation != null;
        }
    }
}