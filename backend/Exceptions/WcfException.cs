using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    public class WcfException : Exception
    {
        protected Error _error;

        public WcfException(HttpStatusCode code, string description)
        {
            this._error = new Error(code, description);
        }

        public WcfException(HttpStatusCode code, string error, string description)
        {
            this._error = new Error(code, error, description);
        }

        public WcfException(Error error)
        {
            this._error = error;
        }

        public Error Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public override string Message
        {
            get { return string.Format("{0} {1}. {2}", Error.Code, Error.Name, Error.Description); }
        }
    }
}