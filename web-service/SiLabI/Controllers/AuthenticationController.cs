using SiLabI.Data;
using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Authentication Logic
    /// </summary>
    public class AuthenticationController
    {
        private UserDataAccess userDAO;

        /// <summary>
        /// Creates a new AuthenticationController.
        /// </summary>
        public AuthenticationController()
        {
            userDAO = new UserDataAccess();
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
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Credenciales Inválidos");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            var response = new AuthenticationResponse();
            var table = userDAO.authenticate(request.Username, request.Password);

            if (table.Rows.Count == 0)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "Credenciales Inválidos");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            var row = table.Rows[0];

            var payload = new Dictionary<string, object>()
            {
                { "id", Convert.ToInt32(row["PK_User_Id"]) },
                { "username", Convert.ToString(row["Username"]) },
                { "type", Convert.ToString(row["Type"]) }
            };

            response.AccessToken = Token.Encode(payload);
            response.Id = Convert.ToInt32(row["PK_User_Id"]);
            response.Name = Convert.ToString(row["Name"]);
            response.LastName1 = Convert.ToString(row["Last_Name_1"]);
            response.LastName2 = Convert.ToString(row["Last_Name_2"]);
            response.Gender = Convert.ToString(row["Gender"]);
            response.Email = Convert.ToString(row["Email"]);
            response.Phone = Convert.ToString(row["Phone"]);
            response.Username = Convert.ToString(row["Username"]);
            response.CreatedAt = Convert.ToDateTime(row["Created_At"]);
            response.UpdatedAt = Convert.ToDateTime(row["Updated_At"]);

            return response;
        }
    }
}