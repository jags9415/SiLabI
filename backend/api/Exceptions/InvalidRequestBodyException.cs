using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    /// <summary>
    /// This exception is thrown when an user sends a request with an invalid body.
    /// </summary>
    public class InvalidRequestBodyException : BaseException
    {
        /// <summary>
        /// Create a new InvalidRequestBodyException.
        /// </summary>
        public InvalidRequestBodyException() : base(HttpStatusCode.BadRequest, "MissingParameter", "Cuerpo de la solicitud inválido.") { }
    }
}