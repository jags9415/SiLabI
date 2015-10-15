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
using SiLabI.Exceptions;
using SiLabI.Model.Request;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Authentication logic.
    /// </summary>
    public class AuthenticationController
    {
        private AuthenticationDataAccess _AuthenticationDA;
        private Dictionary<IPAddress, Attempt> _clientAttempts;

        /// <summary>
        /// Creates a new AuthenticationController.
        /// <param name="maxAttempts">The maximum amount of failed attempts that a user can perform.</param>
        /// <param name="hoursBlocked">The number of hours that a user is blocked after exceed the maximum attempts.</param>
        /// </summary>
        public AuthenticationController(int maxAttempts, int hoursBlocked)
        {
            _AuthenticationDA = new AuthenticationDataAccess();
            _clientAttempts = new Dictionary<IPAddress, Attempt>();

            MaxAttempts = maxAttempts;
            HoursBlocked = hoursBlocked;
        }

        /// <summary>
        /// Creates a new AuthenticationController.
        /// </summary>
        public AuthenticationController() : this(5, 1) { }

        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="request">An authentication request.</param>
        /// <returns>The authentication response.</returns>
        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            IPAddress ip = GetClientIp();

            if (IsClientBlocked(ip))
            {
                throw new UnathorizedOperationException(String.Format("Ha excedido el número de intentos posibles. Intente de nuevo dentro de {0} hora (s)", HoursBlocked));
            }

            var table = _AuthenticationDA.Authenticate(request.Username, request.Password);

            if (table.Rows.Count == 0)
            {
                // Increment the client attempts.
                if (_clientAttempts.ContainsKey(ip))
                {
                    _clientAttempts[ip].Count = _clientAttempts[ip].Count + 1;
                }
                else
                {
                    _clientAttempts[ip] = new Attempt(1);
                }

                // Throw exception.
                if (_clientAttempts[ip].Count >= MaxAttempts)
                {
                    throw new UnathorizedOperationException(String.Format("Ha excedido el número de intentos posibles. Intentelo de nuevo dentro de {0} hora (s)", HoursBlocked));
                }
                else
                {
                    throw new SiLabIException(HttpStatusCode.BadRequest, "InvalidCredentials", String.Format("Credenciales inválidos. Intentos restantes: {0}", MaxAttempts - _clientAttempts[ip].Count));
                }
            }

            _clientAttempts.Remove(ip);
            User user = User.Parse(table.Rows[0]);

            var payload = new Dictionary<string, object>()
            {
                { "id", user.Id },
                { "username", user.Username },
                { "type", user.Type }
            };

            var response = new AuthenticationResponse();
            response.AccessToken = Token.Encode(payload);
            response.User = user;

            return response;
        }

        /// <summary>
        /// Get the current client IP address.
        /// </summary>
        /// <returns>The current client IP address as a string.</returns>
        private IPAddress GetClientIp()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return IPAddress.Parse(endpoint.Address);
        }

        /// <summary>
        /// Check if a client is blocked.
        /// Also removes the blocked clients that has to be unlocked.
        /// </summary>
        /// <param name="ip">The ip address of the client.</param>
        /// <returns>True if the client is blocked.</returns>
        private bool IsClientBlocked(IPAddress ip)
        {
            foreach (var entry in _clientAttempts.Where(entry => (DateTime.Now - entry.Value.DateTime).TotalHours > HoursBlocked).ToList())
            {
                _clientAttempts.Remove(entry.Key);
            }

            return _clientAttempts.ContainsKey(ip) && _clientAttempts[ip].Count >= MaxAttempts;
        }

        /// <summary>
        /// The maximum amount of failed attempts that a user can perform.
        /// </summary>
        public int MaxAttempts { get; set; }

        /// <summary>
        /// The number of hours that a user is blocked after exceed the maximum attempts.
        /// </summary>
        public int HoursBlocked { get; set; }
    }

    /// <summary>
    /// A failed authentication attempt.
    /// </summary>
    public class Attempt
    {
        private int _count;
        private DateTime _datetime;

        /// <summary>
        /// Create a new Attempt.
        /// </summary>
        /// <param name="count">The initial number of failed attempts.</param>
        public Attempt(int count = 0)
        {
            _count = count;
            _datetime = DateTime.Now;
        }

        /// <summary>
        /// The datetime of the last failed attempt.
        /// </summary>
        public DateTime DateTime
        {
            get { return _datetime; }
        }

        /// <summary>
        /// The number of failed attempts performed.
        /// </summary>
        public int Count
        {
            get { return _count; }
            set {
                _count = value;
                _datetime = DateTime.Now;
            }
        }
    }
}
