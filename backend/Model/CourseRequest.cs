using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class CourseRequest : BaseRequest
    {
        protected Course _course;

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
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid() && Course != null;
        }
    }
}