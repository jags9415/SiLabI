using SiLabI.Exceptions;
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
    /// A POST request body to the endpoint /authenticate
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
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new MissingParameterException("username");
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
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new MissingParameterException("password");
                }
                _password = value;
            }
            get { return _password; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public virtual bool IsValid()
        {
            return Username != null && Password != null;
        }
    }
}