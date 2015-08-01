using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Authentication Logic
    /// </summary>
    public class AuthenticationController
    {
        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="request">An authentication request.</param>
        /// <returns>The authentication response.</returns>
        public AuthenticationResponse authenticate(AuthenticationRequest request)
        {
            var auth = new AuthenticationResponse();

            var payload = new Dictionary<string, object>()
            {
                { "id", 19082 },
                { "username", request.username },
                { "name", "Jhon" },
                { "last_name_1", "Doe" },
                { "last_name_2", "Doe" },
                { "email", "jhondoe@example.com" },
                { "gender", "male" }
            };

            auth.access_token = Token.encode(payload);

            return auth;
        }
    }
}