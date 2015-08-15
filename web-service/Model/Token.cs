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
            return JWT.JsonWebToken.DecodeToObject(token, Token.secret) as Dictionary<string, object>;
        }

        /// <summary>
        /// Check if a token is valid for a specific user.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="type">The user type.</param>
        /// <returns>The token validity.</returns>
        public static bool IsValidToken(string token, UserType type)
        {
            if (token == null) return false;
            string permission;

            try
            {
                Dictionary<string, object> payload = Token.Decode(token);
                permission = payload["permission"].ToString();
            }
            catch (JWT.SignatureVerificationException)
            {
                return false;
            }

            switch (permission)
            {
                case "admin":
                    return true;
                case "operator":
                    return type != UserType.Admin;
                case "professor":
                    return type == UserType.Professor;
                case "student":
                    return type == UserType.Student;
                default:
                    return false;
            }
        }

        public static void CheckToken(string token, UserType type)
        {
            if (!Token.IsValidToken(token, type))
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.Unauthorized, "No tiene permiso para acceder a este contenido.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
        }
    }
}