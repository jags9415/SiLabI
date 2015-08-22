using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    /// <summary>
    /// This exception is thrown when a user do not send an required parameter.
    /// </summary>
    public class MissingParameterException : WcfException
    {
        protected string _parameter;

        /// <summary>
        /// Create a new MissingParameterException.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        public MissingParameterException(string parameter) 
            : base(HttpStatusCode.BadRequest, "MissingParameter", string.Format("Parámetro obligatorio '{0}'.", parameter))
        {
            this._parameter = parameter;
        }
    }
}