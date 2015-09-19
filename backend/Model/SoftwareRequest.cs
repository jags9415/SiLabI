using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    [DataContract]
    public class SoftwareRequest : BaseRequest
    {
        protected Software _software;

        /// <summary>
        /// The software.
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
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid() && Software != null;
        }
    }
}