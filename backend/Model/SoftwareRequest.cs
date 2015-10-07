using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A POST or PUT request body to the endpoints /software
    /// </summary>
    [DataContract]
    public class SoftwareRequest : BaseRequest
    {
        protected Software _software;

        /// <summary>
        /// The software data.
        /// </summary>
        [DataMember(Name = "software")]
        public Software Software
        {
            set
            {
                if (value == null)
                {
                    throw new MissingParameterException("software");
                }
                _software = value;
            }
            get { return _software; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Software != null;
        }
    }
}