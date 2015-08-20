using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Model
{
    /// <summary>
    /// Authentication request.
    /// </summary>
    [DataContract]
    public class AuthenticationRequest
    {
        protected string _username;
        protected string _password;

        /// <summary>
        /// The username.
        /// </summary>
        [DataMember(Name = "username")]
        public string Username
        {
            set
            {
                if (value == null)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "MissingParameter", "El nombre de usuario es obligatorio.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _username = value;
            }
            get { return _username; }
        }

        /// <summary>
        /// The password hashed with SHA256.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password
        {
            set
            {
                if (value == null)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "MissingParameter", "La contraseña es obligatoria.");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }
                _password = value;
            }
            get { return _password; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            return Username != null && Password != null;
        }
    }
}