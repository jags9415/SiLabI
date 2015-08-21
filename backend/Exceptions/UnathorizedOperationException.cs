using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    public class UnathorizedOperationException : WcfException
    {
        public UnathorizedOperationException() : base(HttpStatusCode.Unauthorized, "No tiene permisos para realizar esta operación.") { }
    }
}