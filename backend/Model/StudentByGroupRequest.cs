using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A POST, PUT or DELETE request body to the endpoints /groups/{id}/students
    /// </summary>
    [DataContract]
    public class StudentByGroupRequest : BaseRequest
    {
        protected List<string> _students;

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
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Students != null;
        }
    }
}