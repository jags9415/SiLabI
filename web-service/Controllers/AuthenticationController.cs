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
        private UserDataAccess _UserDAO;

        /// <summary>
        /// Creates a new AuthenticationController.
        /// </summary>
        public AuthenticationController()
        {
            _UserDAO = new UserDataAccess();
        }

        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="request">An authentication request.</param>
        /// <returns>The authentication response.</returns>
        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            if (request.Username == null || request.Password == null)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "MissingCredentials", "Faltan ciertos campos obligatorios");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            var response = new AuthenticationResponse();

            try
            {
                var table = _UserDAO.Authenticate(request.Username, request.Password);
                
                if (table.Rows.Count == 0)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "InvalidCredentials", "Credenciales inválidos");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }

                User user = _UserDAO.ParseUser(table.Rows[0]);

                var payload = new Dictionary<string, object>()
                {
                    { "id", Convert.ToInt32(user.Id) },
                    { "username", Convert.ToString(user.Username) },
                    { "type", Convert.ToString(table.Rows[0]["type"]) }
                };

                response.AccessToken = Token.Encode(payload);
                response.User = user;
            }
            catch (SqlException)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al recuperar los datos.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            return response;
        }
    }
}