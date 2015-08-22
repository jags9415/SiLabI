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
    public class UnathorizedOperationException : WcfException
    {
        /// <summary>
        /// Creates a new UnathorizedOperationException.
        /// </summary>
        public UnathorizedOperationException() : base(HttpStatusCode.Unauthorized, "No tiene permisos para realizar esta operación.") { }
    }
}