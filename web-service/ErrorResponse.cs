
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Base class for all the responses in the service.
    /// </summary>
    [DataContract]
    public class ErrorResponse
    {
        /// <summary>
        /// Creates a new ErrorResponse.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="description">The description.</param>
        public ErrorResponse(HttpStatusCode code, string description)
        {
            this.Code = code;
            this.Error = code.ToString();
            this.Description = description;
        }

        /// <summary>
        /// Creates a new ErrorResponse.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="error">The error.</param>
        /// <param name="description">The description.</param>
        public ErrorResponse(HttpStatusCode code, string error, string description)
        {
            this.Code = code;
            this.Error = error;
            this.Description = description;
        }

        /// <summary>
        /// The error name.
        /// </summary>
        /// <example>NOT_AUTHORIZED</example>
        [DataMember(EmitDefaultValue = false, Name = "error")]
        public string Error { get; set; }

        /// <summary>
        /// The error code.
        /// </summary>
        /// <example>1002</example>
        [DataMember(EmitDefaultValue = false, Name = "code")]
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// The error description
        /// </summary>
        /// <example>The token has expired.</example>
        [DataMember(EmitDefaultValue = false, Name = "description")]
        public string Description { get; set; }
    }
}