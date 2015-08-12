using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Authentication request.
    /// </summary>
    [DataContract]
    public class AuthenticationRequest
    {
        /// <summary>
        /// The username.
        /// </summary>
        [DataMember(Name = "username")]
        public string Username { get; set; }

        /// <summary>
        /// The password hashed with SHA256.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }

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