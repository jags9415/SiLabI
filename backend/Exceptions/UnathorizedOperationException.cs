using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    /// <summary>
    /// This exception is thrown when an user is trying to do an operation but doesn't have the right permissions.
    /// </summary>
    public class UnathorizedOperationException : SiLabIException
    {
        /// <summary>
        /// Creates a new UnathorizedOperationException.
        /// </summary>
        public UnathorizedOperationException(string message) : base(HttpStatusCode.Unauthorized, message) { }

        /// <summary>
        /// Creates a new UnathorizedOperationException.
        /// </summary>
        public UnathorizedOperationException() : this("No tiene permisos para realizar esta operación.") { }
    }
}