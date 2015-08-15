using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Authentication response.
    /// </summary>
    [DataContract]
    public class AuthenticationResponse : User
    {
        /// <summary>
        /// A JWT token.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "access_token")]
        public string AccessToken { get; set; }
    }
}