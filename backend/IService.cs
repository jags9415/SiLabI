﻿using SiLabI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace SiLabI
{
    /// <summary>
    /// The web service interface.
    /// </summary>
    [ServiceContract]
    public interface IService
    {
        /*
         * AUTHENTICATION ENDPOINTS.
         */

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/authenticate/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Authenticate a user in the service.")]
        AuthenticationResponse Authenticate(AuthenticationRequest request);

        /*
         * USER ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/users/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of users.")]
        GetResponse<User> GetUsers(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{username}/?access_token={token}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an user.")]
        User GetUser(string username, string token);

        /*
         * ADMINISTRATOR ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/administrators/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of administrators.")]
        GetResponse<User> GetAdministrators(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/administrators/{id}/?access_token={token}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an administrator.")]
        User GetAdministrator(string id, string token);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/administrators/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create an administrator.")]
        User CreateAdministrator(string id, BaseRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/administrators/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete an administrator.")]
        void DeleteAdministrator(string id, BaseRequest request);

        /*
         * OPERATOR ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/operators/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of operators.")]
        GetResponse<Operator> GetOperators(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/operators/{id}/?access_token={token}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an operator.")]
        Operator GetOperator(string id, string token);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/operators/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create an operator.")]
        Operator CreateOperator(string id, OperatorRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/operators/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete an operator.")]
        void DeleteOperator(string id, BaseRequest request);

        /*
         * STUDENTS ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/students/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of students.")]
        GetResponse<Student> GetStudents(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/students/{username}/?access_token={token}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a student.")]
        Student GetStudent(string username, string token);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/students/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a student.")]
        Student CreateStudent(StudentRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/students/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a student.")]
        Student UpdateStudent(string id, StudentRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/students/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a student.")]
        void DeleteStudent(string id, BaseRequest request);

        /*
         * PROFESSORS ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/professors/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of professors.")]
        GetResponse<User> GetProfessors(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/professors/{username}/?access_token={token}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a professor.")]
        User GetProfessor(string username, string token);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/professors/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a professor.")]
        User CreateProfessor(ProfessorRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/professors/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a professor.")]
        User UpdateProfessor(string id, ProfessorRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/professors/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a professor.")]
        void DeleteProfessor(string id, BaseRequest request);
    }
}
