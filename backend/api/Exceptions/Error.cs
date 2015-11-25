
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace SiLabI.Exceptions
{
    /// <summary>
    /// Contains information about an error.
    /// </summary>
    [DataContract]
    public class Error
    {
        /// <summary>
        /// Creates a new ErrorResponse.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="description">The description.</param>
        public Error(HttpStatusCode code, string description)
        {
            this.Code = code;
            this.Name = code.ToString();
            this.Description = description;
        }

        /// <summary>
        /// Creates a new ErrorResponse.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="name">The error name.</param>
        /// <param name="description">The description.</param>
        public Error(HttpStatusCode code, string name, string description)
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// The error name.
        /// </summary>
        /// <example>NOT_AUTHORIZED</example>
        [DataMember(EmitDefaultValue = false, Name = "error")]
        public string Name { get; set; }

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