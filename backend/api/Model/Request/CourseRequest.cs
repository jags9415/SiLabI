using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model.Request
{
    /// <summary>
    /// A POST or PUT request body to the endpoints /courses
    /// </summary>
    [DataContract]
    public class CourseRequest : BaseRequest
    {
        protected Course _course;

        /// <summary>
        /// The course data.
        /// </summary>
        [DataMember(Name = "course")]
        public Course Course
        {
            set
            {
                if (value == null)
                {
                    throw new MissingParameterException("course");
                }
                _course = value;
            }
            get { return _course; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Course != null;
        }
    }
}