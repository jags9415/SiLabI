using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// An response from the endpoint /authenticate
    /// </summary>
    [DataContract]
    public class AuthenticationResponse
    {
        /// <summary>
        /// A JWT token.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The user data.
        /// </summary>
        [DataMember(EmitDefaultValue = false, Name = "user")]
        public User User { get; set; }
    }
}