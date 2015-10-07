using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Contains the group data in a POST or PUT request to the endpoints /groups
    /// </summary>
    [DataContract]
    public class InnerGroupRequest
    {
        protected int? _number;
        protected string _state;
        protected string _course;
        protected string _professor;
        protected List<string> _students;
        protected Period _period;

        /// <summary>
        /// The group number.
        /// </summary>
        [DataMember(Name = "number")]
        public virtual int? Number
        {
            set { _number = value; }
            get { return _number; }
        }

        /// <summary>
        /// The group state.
        /// </summary>
        [DataMember(Name = "state")]
        public virtual string State
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        /// The course code.
        /// </summary>
        [DataMember(Name = "course")]
        public virtual string Course
        {
            set { _course = value; }
            get { return _course; }
        }

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
        /// The period.
        /// </summary>
        [DataMember(Name = "period")]
        public virtual Period Period
        {
            set { _period = value; }
            get { return _period; }
        }

        /// <summary>
        /// The list of students usernames.
        /// </summary>
        [DataMember(Name = "students")]
        public virtual List<string> Students
        {
            set { _students = value; }
            get { return _students; }
        }

        /// <summary>
        /// Check if the object properties are valid for a create operation.
        /// </summary>
        /// <returns>True if the object properties are valid for a create operation.</returns>
        public bool IsValidForCreate()
        {
            bool valid = true;

            valid &= !string.IsNullOrWhiteSpace(Course);
            valid &= !string.IsNullOrWhiteSpace(Professor);
            valid &= (Number != null && Number > 0);
            valid &= (Period != null && Period.isValidForCreate());

            return valid;
        }

        /// <summary>
        /// Check if the object properties are valid for an update operation.
        /// </summary>
        /// <returns>True if the object properties are valid for an update operation.</returns>
        public bool IsValidForUpdate()
        {
            bool valid = true;

            if (Course != null) valid &= !string.IsNullOrWhiteSpace(Course);
            if (Professor != null) valid &= !string.IsNullOrWhiteSpace(Professor);
            if (State != null) valid &= !string.IsNullOrWhiteSpace(State);
            if (Number != null) valid &= (Number > 0);
            if (Period != null) valid &= Period.isValidForUpdate();

            return valid;
        }
    }

    /// <summary>
    /// A POST or PUT request body to the endpoints /groups
    /// </summary>
    [DataContract]
    public class GroupRequest : BaseRequest
    {
        protected InnerGroupRequest _group;

        /// <summary>
        /// The group data.
        /// </summary>
        [DataMember(Name = "group")]
        public virtual InnerGroupRequest Group
        {
            set { _group = value; }
            get { return _group; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Group != null;
        }
    }
}