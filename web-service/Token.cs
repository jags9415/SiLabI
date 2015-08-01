using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SiLabI
{
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
        public static string encode(Dictionary<string, object> payload)
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

            return JWT.JsonWebToken.Encode(payload, Token.secret, JWT.JwtHashAlgorithm.HS256); ;
        }

        /// <summary>
        /// Decode the data stored in a JWT token.
        /// </summary>
        /// <param name="token">The JWT token.</param>
        /// <returns>The data.</returns>
        public static Dictionary<string, object> decode(string token)
        {
            return JWT.JsonWebToken.DecodeToObject(token, Token.secret) as Dictionary<string, object>;
        }
    }
}