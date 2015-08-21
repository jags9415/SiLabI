using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SiLabI.Exceptions
{
    public class InvalidRequestBodyException : WcfException
    {
        public InvalidRequestBodyException() : base(HttpStatusCode.BadRequest, "MissingParameter", "Cuerpo de la solicitud inválido.") { }
    }
}