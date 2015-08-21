using SiLabI.Model;
using SiLabI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;
using SiLabI.Util;
using System.Data.SqlClient;
using SiLabI.Exceptions;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Authentication logic.
    /// </summary>
    public class AuthenticationController
    {
        private UserDataAccess _UserDA;

        /// <summary>
        /// Creates a new AuthenticationController.
        /// </summary>
        public AuthenticationController()
        {
            _UserDA = new UserDataAccess();
        }

        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="request">An authentication request.</param>
        /// <returns>The authentication response.</returns>
        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            var response = new AuthenticationResponse();

            var table = _UserDA.Authenticate(request.Username, request.Password);
                
            if (table.Rows.Count == 0)
            {
                throw new WcfException(HttpStatusCode.BadRequest, "InvalidCredentials", "Credenciales inválidos");
            }

            User user = User.Parse(table.Rows[0]);
                
            var payload = new Dictionary<string, object>()
            {
                { "id", user.Id },
                { "username", user.Username },
                { "type", user.Type }
            };

            response.AccessToken = Token.Encode(payload);
            response.User = user;

            return response;
        }
    }
}