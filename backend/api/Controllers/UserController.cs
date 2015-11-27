﻿using SiLabI.Data;
using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
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
    /// Perform CRUD operations for Users.
    /// </summary>
    public class UserController : IController<User>
    {
        private UserDataAccess _UserDA;

        /// <summary>
        /// Creates an UserController();
        /// </summary>
        public UserController()
        {
            this._UserDA = new UserDataAccess();
        }

        public PaginatedResponse<User> GetAll(QueryString request, Dictionary<string, object> payload)
        {
            // By default search only active users.
            if (!request.Query.Exists(element => element.Name == "state"))
            {
                Field field = Field.Find(ValidFields.User, "state");
                request.Query.Add(new QueryField(field, Relationship.EQ, "Activo"));
            }

            PaginatedResponse<User> response = new PaginatedResponse<User>();
            DataTable table = _UserDA.GetAll(payload, request);
            int count = _UserDA.GetCount(payload, request);     

            foreach (DataRow row in table.Rows)
            {
                response.Results.Add(User.Parse(row));
            }

            response.CurrentPage = request.Page;
            response.TotalPages = (count + request.Limit - 1) / request.Limit;

            return response;
        }

        public User GetOne(string username, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _UserDA.GetOne(payload, username, request);
            return User.Parse(row);
        }

        public User GetOne(int id, QueryString request, Dictionary<string, object> payload)
        {
            DataRow row = _UserDA.GetOne(payload, id, request);
            return User.Parse(row);
        }

        public User Create(BaseRequest request, Dictionary<string, object> payload)
        {
            throw new InvalidOperationException("An user cannot be created. Create a student or professor instead.");
        }

        public User Update(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            UserRequest userRequest = (request as UserRequest);
            if (userRequest == null || !userRequest.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            if (!userRequest.User.IsValidForUpdate())
            {
                throw new BaseException(HttpStatusCode.BadRequest, "Datos de usuario inválidos.");
            }

            DataRow row = _UserDA.Update(payload, id, userRequest.User);
            return User.Parse(row);
        }

        public void Delete(int id, BaseRequest request, Dictionary<string, object> payload)
        {
            if (request == null || !request.IsValid())
            {
                throw new InvalidRequestBodyException();
            }

            _UserDA.Delete(payload, id);
        }
    }
}