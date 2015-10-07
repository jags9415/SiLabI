using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A POST or PUT request body to the endpoints /students
    /// </summary>
    [DataContract]
    public class StudentRequest : BaseRequest
    {
        protected Student _student;

        /// <summary>
        /// The student data.
        /// </summary>
        [DataMember(Name="student")]
        public Student Student
        {
            set
            {
                if (value == null)
                {
                    throw new MissingParameterException("student");
                }
                _student = value;
            }
            get { return _student; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Student != null;
        }
    }
}