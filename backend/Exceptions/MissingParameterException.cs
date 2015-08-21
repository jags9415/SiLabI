using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    public class MissingParameterException : WcfException
    {
        protected string _parameter;

        public MissingParameterException(string parameter) 
            : base(HttpStatusCode.BadRequest, "MissingParameter", string.Format("Parámetro obligatorio '{0}'.", parameter))
        {
            this._parameter = parameter;
        }
    }
}