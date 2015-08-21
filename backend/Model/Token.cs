using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Model
{
    public enum UserType { Student, Professor, Operator, Admin }

    /// <summary>
    /// Encode and Decode JWT tokens.
    /// </summary>
    public class Token
    {
        private static string secret = ConfigurationManager.AppSettings["JWTsecret"];

        /// <summary>
        /// Encode a payload into a JWT token.
        /// </summary>
        /// <param name="payload">The data to encode.</param>
        /// <returns>A JWT token.</returns>
        public static string Encode(Dictionary<string, object> payload)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = DateTime.UtcNow;
            var tomorrow = now.AddDays(1);

            // Add the Issued At claim
            var iat = Math.Round((now - epoch).TotalSeconds);
            payload.Add("iat", iat);

            // Add the Expiration claim
            var exp = Math.Round((tomorrow - epoch).TotalSeconds);
            payload.Add("exp", exp);

            return JWT.JsonWebToken.Encode(payload, Token.secret, JWT.JwtHashAlgorithm.HS256);
        }

        /// <summary>
        /// Decode the data stored in a JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>The data.</returns>
        public static Dictionary<string, object> Decode(string token)
        {
            return null;

            /*
            if (token == null)
            {
                throw new MissingParameterException("access_token");
            }

            try
            {
                return JWT.JsonWebToken.DecodeToObject(token, Token.secret) as Dictionary<string, object>;
            }
            catch (JWT.SignatureVerificationException)
            {
                throw new InvalidParameterException("access_token");
            }
             */
        }

        /// <summary>
        /// Check if a token is valid for a specific user.
        /// </summary>
        /// <param name="payload">The token payload.</param>
        /// <param name="type">The user type.</param>
        /// <returns>The token validity.</returns>
        public static void CheckPayload(Dictionary<string, object> payload, UserType type)
        {
            /*
            string permission = payload["permission"].ToString();
            bool valid;

            switch (permission)
            {
                case "admin":
                    valid = true;
                    break;
                case "operator":
                    valid = type != UserType.Admin;
                    break;
                case "professor":
                    valid = type == UserType.Professor;
                    break;
                case "student":
                    valid = type == UserType.Student;
                    break;
                default:
                    valid = false;
                    break;
            }

            if (!valid)
            {
                throw new UnathorizedException();
            }
             */
        }
    }
}