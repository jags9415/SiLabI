using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class LaboratoryRequest : BaseRequest
    {
        private Laboratory _laboratory;

        /// <summary>
        /// The laboratory.
        /// </summary>
        [DataMember(Name = "laboratory")]
        public virtual Laboratory Laboratory
        {
            set { _laboratory = value; }
            get { return _laboratory; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid() && Laboratory != null;
        }
    }
}