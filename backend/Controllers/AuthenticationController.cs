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
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Cuerpo de la solicitud inválido.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            var response = new AuthenticationResponse();

            try
            {
                var table = _UserDA.Authenticate(request.Username, request.Password);
                
                if (table.Rows.Count == 0)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidCredentials", "Credenciales inválidos");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
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
            }
            catch (SqlException e)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.InternalServerError, e.Message);
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            return response;
        }
    }
}