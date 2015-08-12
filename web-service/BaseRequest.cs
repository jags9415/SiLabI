using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Base class for all request in the service.
    /// </summary>
    [DataContract]
    public class BaseRequest
    {
        /// <summary>
        /// The JWT access token.
        /// </summary>
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
    }
}