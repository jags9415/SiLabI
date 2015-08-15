using SiLabI.Data;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web;

namespace SiLabI.Controllers
{
    /// <summary>
    /// Administrator logic.
    /// </summary>
    public class AdministratorController
    {
        private AdministratorDataAccess _AdminDA;
        private UserDataAccess _UserDA;

        /// <summary>
        /// Creates a new AdministratorController.
        /// </summary>
        public AdministratorController()
        {
            this._AdminDA = new AdministratorDataAccess();
            this._UserDA = new UserDataAccess();
        }

        /// <summary>
        /// Retrieves a list of administrators based on a query.
        /// </summary>
        /// <param name="request">The query.</param>
        /// <returns>The list of administrators.</returns>
        public List<User> GetAdministrators(QueryString request)
        {
            //Token.CheckToken(request.AccessToken, UserType.Operator);

            List<User> list = new List<User>();
            try
            {
                DataTable table = _AdminDA.GetAdministrators(request);
                
                foreach (DataRow row in table.Rows)
                {
                    list.Add(_UserDA.ParseUser(row));
                }
            }
            catch (SqlException)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al recuperar los datos.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }

            return list;
        }

        /// <summary>
        /// Get an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="token">The access token.</param>
        /// <returns>The administrator.</returns>
        public User GetAdministrator(int id, string token)
        {
            //Token.CheckToken(token, UserType.Operator);
            return FetchAdministrator(id);
        }

        /// <summary>
        /// Creates an administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The user data.</returns>
        public User CreateAdministrator(int id, BaseRequest request)
        {
            //Token.CheckToken(request.AccessToken, UserType.Admin);
            try
            {
                _AdminDA.CreateAdministrator(id);
            }
            catch (SqlException)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al actualizar los datos.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return FetchAdministrator(id);
        }

        /// <summary>
        /// Deletes an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <param name="request">The request.</param>
        public User DeleteAdministrator(int id, BaseRequest request)
        {
            //Token.CheckToken(request.AccessToken, UserType.Admin);
            User admin = FetchAdministrator(id);
            try
            {
                _AdminDA.DeleteAdministrator(id);
            }
            catch (SqlException)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al eliminar los datos.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
            return admin;
        }

        /// <summary>
        /// Retrieves an administrator.
        /// </summary>
        /// <param name="id">The user identification.</param>
        /// <returns>The user data.</returns>
        private User FetchAdministrator(int id)
        {
            try
            {
                DataTable table = _AdminDA.GetAdministrator(id);

                if (table.Rows.Count == 0)
                {
                    ErrorResponse error = new ErrorResponse(HttpStatusCode.BadRequest, "El identificador ingresado no corresponde a un administrador");
                    throw new WebFaultException<ErrorResponse>(error, error.Code);
                }

                return _UserDA.ParseUser(table.Rows[0]);
            }
            catch (SqlException)
            {
                ErrorResponse error = new ErrorResponse(HttpStatusCode.InternalServerError, "Ocurrió un error al recuperar los datos.");
                throw new WebFaultException<ErrorResponse>(error, error.Code);
            }
        }
    }
}