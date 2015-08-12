using SiLabI.Model;
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
        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/authenticate",
            RequestFormat = WebMessageFormat.Json),
        Description("Authenticate a user in the service.")]
        AuthenticationResponse Authenticate(AuthenticationRequest request);

        /*
        [OperationContract]
        [WebGet(UriTemplate = "/students?access_token={token}&q={query}&offset={offset}&limit={limit}&sort={sort}&fields={fields}"),
        Description("Retrieve a list of students.")]
        List<Student> GetStudents(string token, string query, string offset, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/student/{id}?access_token={token}"),
        Description("Retrieve a student.")]
        Student GetStudent(string id, string token);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/student",
            RequestFormat = WebMessageFormat.Json),
        Description("Create a student.")]
        Student CreateStudent(StudentRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/student/{id}",
            RequestFormat = WebMessageFormat.Json),
        Description("Update a student.")]
        Student UpdateStudent(string id, StudentRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/student/{id}",
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a student.")]
        void DeleteStudent(string id, BaseRequest request);
        */
    }
}
