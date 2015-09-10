using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Request body for creating and operator.
    /// </summary>
    [DataContract]
    public class OperatorRequest : BaseRequest
    {
        private Period _period;

        /// <summary>
        /// The period.
        /// </summary>
        [DataMember(Name = "period")]
        public Period Period
        {
            get { return _period; }
            set { _period = value; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return base.IsValid() && Period != null && Period.isValidForCreate();
        }
    }
}