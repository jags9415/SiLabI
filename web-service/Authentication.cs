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
        [DataMember]
        public string username { get; set; }

        /// <summary>
        /// The password hashed with SHA256.
        /// </summary>
        [DataMember]
        public string password { get; set; }
    }

    /// <summary>
    /// Authentication response.
    /// </summary>
    [DataContract]
    public class AuthenticationResponse : BaseResponse
    {
        /// <summary>
        /// A JWT token.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string access_token { get; set; }
    }
}