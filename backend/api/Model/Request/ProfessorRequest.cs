using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model.Request
{
    /// <summary>
    /// A POST or PUT request body to the endpoints /professors
    /// </summary>
    [DataContract]
    public class ProfessorRequest : BaseRequest
    {
        protected User _professor;

        /// <summary>
        /// The professor data.
        /// </summary>
        [DataMember(Name = "professor")]
        public User Professor
        {
            set
            {
                if (value == null)
                {
                    throw new MissingParameterException("professor");
                }
                _professor = value;
            }
            get { return _professor; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Professor != null;
        }
    }
}