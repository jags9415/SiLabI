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
        private UserDataAccess dao;

        /// <summary>
        /// Creates a new AuthenticationController.
        /// </summary>
        public AuthenticationController()
        {
            dao = new UserDataAccess();
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
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Faltan ciertos campos obligatorios.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            var response = new AuthenticationResponse();

            try
            {
                var table = dao.Authenticate(request.Username, request.Password);
                
                if (table.Rows.Count == 0)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Credenciales Inválidos");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }

                var row = table.Rows[0];

                var payload = new Dictionary<string, object>()
                {
                    { "id", Convert.ToInt32(row["id"]) },
                    { "username", Convert.ToString(row["username"]) },
                    { "type", Convert.ToString(row["type"]) }
                };

                response.AccessToken = Token.Encode(payload);
                response.Id = Converter.ToInt32(row["id"]);
                response.Name = Converter.ToString(row["name"]);
                response.LastName1 = Converter.ToString(row["last_name_1"]);
                response.LastName2 = Converter.ToString(row["last_name_2"]);
                response.Gender = Converter.ToString(row["gender"]);
                response.Email = Converter.ToString(row["email"]);
                response.Phone = Converter.ToString(row["phone"]);
                response.Username = Converter.ToString(row["username"]);
                response.CreatedAt = Converter.ToDateTime(row["created_at"]);
                response.UpdatedAt = Converter.ToDateTime(row["updated_at"]); 
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