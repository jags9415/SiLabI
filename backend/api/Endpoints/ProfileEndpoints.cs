using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        /// <summary>
        /// Get an user profile.
        /// </summary>
        /// <param name="token">The access token.</param>
        /// <param name="fields">The request.</param>
        /// <returns>The user profile.</returns>
        public User GetProfile(string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Any);

            QueryString request = new QueryString(ValidFields.User);
            request.AccessToken = token;
            request.ParseFields(fields);

            return _UserController.GetOne(payload["username"] as string, request, payload);
        }

        public User UpdateProfile(UserRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Any);
            int id = (int) payload["id"];

            request.User.Name = null;
            request.User.LastName1 = null; 
            request.User.LastName2 = null;
            request.User.Username = null;
            request.User.State = null;

            return _UserController.Update(id, request, payload);
        }
    }
}