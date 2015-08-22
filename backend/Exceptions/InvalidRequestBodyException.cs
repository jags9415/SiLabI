using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    /// <summary>
    /// This exception is thrown when an user send a request with no body.
    /// </summary>
    public class InvalidRequestBodyException : WcfException
    {
        /// <summary>
        /// Create a new InvalidRequestBodyException.
        /// </summary>
        public InvalidRequestBodyException() : base(HttpStatusCode.BadRequest, "MissingParameter", "Cuerpo de la solicitud inválido.") { }
    }
}