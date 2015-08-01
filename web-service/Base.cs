using System;
using System.Collections.Generic;
using System.Linq;
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
        [DataMember]
        public string access_token { get; set; }
    }

    /// <summary>
    /// Base class for all the responses in the service.
    /// </summary>
    [DataContract]
    public class BaseResponse
    {
        /// <summary>
        /// The error name.
        /// </summary>
        /// <example>NOT_AUTHORIZED</example>
        [DataMember(EmitDefaultValue = false)]
        public string error { get; set; }

        /// <summary>
        /// The error code.
        /// </summary>
        /// <example>1002</example>
        [DataMember(EmitDefaultValue = false)]
        public int code { get; set; }

        /// <summary>
        /// The error description
        /// </summary>
        /// <example>The token has expired.</example>
        [DataMember(EmitDefaultValue = false)]
        public string description { get; set; }
    }
}