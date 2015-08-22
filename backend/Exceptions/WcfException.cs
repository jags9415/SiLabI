using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    /// <summary>
    /// Base exception class.
    /// </summary>
    public class WcfException : Exception
    {
        protected Error _error;

        /// <summary>
        /// Creates a new WcfException.
        /// </summary>
        /// <param name="code">The HTTP status code.</param>
        /// <param name="description">The description.</param>
        public WcfException(HttpStatusCode code, string description)
        {
            this._error = new Error(code, description);
        }

        /// <summary>
        /// Creates a new WcfException.
        /// </summary>
        /// <param name="code">The HTTP status code.</param>
        /// <param name="error">The error name.</param>
        /// <param name="description">The description.</param>
        public WcfException(HttpStatusCode code, string error, string description)
        {
            this._error = new Error(code, error, description);
        }

        /// <summary>
        /// Creates a new WcfException.
        /// </summary>
        /// <param name="error">The error data.</param>
        public WcfException(Error error)
        {
            this._error = error;
        }

        /// <summary>
        /// The error data.
        /// </summary>
        public Error Error
        {
            get { return _error; }
            set { _error = value; }
        }

        /// <summary>
        /// The message string.
        /// </summary>
        public override string Message
        {
            get { return string.Format("{0} {1}. {2}", Error.Code, Error.Name, Error.Description); }
        }
    }
}