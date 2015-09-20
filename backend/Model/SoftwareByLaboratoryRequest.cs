using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class SoftwareByLaboratoryRequest : BaseRequest
    {
        protected List<string> _software;

        /// <summary>
        /// The software list.
        /// </summary>
        [DataMember(Name = "software")]
        public virtual List<string> Software
        {
            set { _software = value; }
            get { return _software; }
        }

        public override bool IsValid()
        {
            return base.IsValid() && Software != null;
        }
    }
}