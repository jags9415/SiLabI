using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model.Request
{
    /// <summary>
    /// A PUT request body to the endpoints /me
    /// </summary>
    [DataContract]
    public class UserRequest : BaseRequest
    {
        protected User _user;

        /// <summary>
        /// The user data.
        /// </summary>
        [DataMember(Name = "user")]
        public User User
        {
            set
            {
                if (value == null)
                {
                    throw new MissingParameterException("user");
                }
                _user = value;
            }
            get { return _user; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public override bool IsValid()
        {
            return base.IsValid() && User != null;
        }
    }
}