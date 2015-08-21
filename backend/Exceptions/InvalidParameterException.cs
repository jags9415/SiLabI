using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    public class InvalidParameterException : WcfException
    {
        protected string _parameter;

        public InvalidParameterException(string parameter) 
            : base(HttpStatusCode.BadRequest, "InvalidParameter", string.Format("Parámetro inválido '{0}'", parameter))
        {
            this._parameter = parameter;
        }

        public InvalidParameterException(string parameter, string description)
            : base(HttpStatusCode.BadRequest, "InvalidParameter", string.Format("Parámetro inválido '{0}'. {1}.", parameter, description))
        {
            this._parameter = parameter;
        }
    }
}