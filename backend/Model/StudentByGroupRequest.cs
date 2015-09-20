using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class StudentByGroupRequest : BaseRequest
    {
        protected List<string> _students;

        /// <summary>
        /// The list of students.
        /// </summary>
        [DataMember(Name = "students")]
        public virtual List<string> Students
        {
            set { _students = value; }
            get { return _students; }
        }

        public override bool IsValid()
        {
            return base.IsValid() && Students != null;
        }
    }
}