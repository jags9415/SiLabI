using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    /// <summary>
    /// This exception is thrown when a parameter doesn't have the right format.
    /// </summary>
    public class InvalidParameterException : SiLabIException
    {
        protected string _parameter;

        /// <summary>
        /// Create a new InvalidParameterException.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        public InvalidParameterException(string parameter) 
            : base(HttpStatusCode.BadRequest, "InvalidParameter", string.Format("Parámetro inválido '{0}'", parameter))
        {
            this._parameter = parameter;
        }

        /// <summary>
        /// Create a new InvalidParameterException.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        /// <param name="description">The description.</param>
        public InvalidParameterException(string parameter, string description)
            : base(HttpStatusCode.BadRequest, "InvalidParameter", string.Format("Parámetro inválido '{0}'. {1}.", parameter, description))
        {
            this._parameter = parameter;
        }
    }
}