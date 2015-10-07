using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// A POST request body to the endpoints /operators
    /// </summary>
    [DataContract]
    public class OperatorRequest : BaseRequest
    {
        private Period _period;
        private int _id;

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
        /// The student identity.
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && Period != null && Period.isValidForCreate();
        }
    }
}