using SiLabI.Exceptions;
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
    /// <summary>
    /// The types of users.
    /// </summary>
    public enum UserType { Any, Student, Professor, Operator, Administrator }

    /// <summary>
    /// Encode and Decode JWT tokens.
    /// </summary>
    public class Token
    {
        private static string key = ConfigurationManager.AppSettings["jwtKey"];

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

            return JWT.JsonWebToken.Encode(payload, Token.key, JWT.JwtHashAlgorithm.HS256);
        }

        /// <summary>
        /// Decode the data stored in a JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>The data.</returns>
        public static Dictionary<string, object> Decode(string token)
        {
            if (token == null)
            {
                throw new MissingParameterException("access_token");
            }

            try
            {
                return JWT.JsonWebToken.DecodeToObject(token, Token.key) as Dictionary<string, object>;
            }
            catch (JWT.SignatureVerificationException e)
            {
                if (e.Message == "Token has expired.") 
                    throw new InvalidParameterException("access_token", "El token de acceso ha expirado.");
                else
                    throw new InvalidParameterException("access_token", "El token de acceso es inválido.");
            }
            catch (Exception)
            {
                throw new InvalidParameterException("access_token", "El token de acceso es inválido.");
            }
        }

        /// <summary>
        /// Check if a token is valid for a specific user.
        /// </summary>
        /// <param name="payload">The token payload.</param>
        /// <param name="type">The user type.</param>
        /// <returns>The token validity.</returns>
        public static void CheckPayload(Dictionary<string, object> payload, UserType type)
        {
            if (!payload.ContainsKey("id") || !payload.ContainsKey("username") || !payload.ContainsKey("type"))
            {
                throw new InvalidParameterException("access_token", "Los datos del token de acceso están incompletos.");
            }

            bool valid;
            string user = payload["type"].ToString();

            switch (type)
            {
                case UserType.Any:
                    valid = (user == "Administrador") || (user == "Operador") || (user == "Docente") || (user == "Estudiante");
                    break;
                case UserType.Administrator:
                    valid = (user == "Administrador");
                    break;
                case UserType.Operator:
                    valid = (user == "Administrador") || (user == "Operador");
                    break;
                case UserType.Professor:
                    valid = (user == "Administrador") || (user == "Operador") || (user == "Docente");
                    break;
                case UserType.Student:
                    valid = (user == "Administrador") || (user == "Operador") || (user == "Estudiante");
                    break;
                default:
                    valid = false;
                    break;
            }

            if (!valid)
            {
                throw new UnathorizedOperationException();
            }
        }
    }
}