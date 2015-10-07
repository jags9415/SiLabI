using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A POST, PUT or DELETE request body to the endpoints /laboratories/{id}/software
    /// </summary>
    [DataContract]
    public class SoftwareByLaboratoryRequest : BaseRequest
    {
        protected List<string> _software;

        /// <summary>
        /// The list of software codes.
        /// </summary>
        [DataMember(Name = "software")]
        public virtual List<string> Software
        {
            set { _software = value; }
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