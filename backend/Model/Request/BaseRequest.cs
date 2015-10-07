using SiLabI.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Model.Request
{
    /// <summary>
    /// Base class for all request in the service.
    /// </summary>
    [DataContract]
    public class BaseRequest
    {
        protected string _accessToken;

        /// <summary>
        /// The JWT access token.
        /// </summary>
        [DataMember(Name = "access_token")]
        public string AccessToken
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new MissingParameterException("access_token");
                }
                _accessToken = value;
            }
            get { return _accessToken; }
        }

        /// <summary>
        /// Check if the object properties are valid.
        /// </summary>
        /// <returns>True if the object properties are valid.</returns>
        public virtual bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(AccessToken);
        }
    }
}