using SiLabI.Model;
using SiLabI.Model.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
         * WELCOME ENDPOINT.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Show a welcome message.")]
        Stream GetWelcomeMessage();

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
         * PROFILE ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/me/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an user profile.")]
        User GetProfile(string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/me/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update an user profile.")]
        User UpdateProfile(UserRequest request);

        /*
         * USER ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/users/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of users.")]
        PaginatedResponse<User> GetUsers(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/users/{username}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an user.")]
        User GetUser(string username, string token, string fields);

        /*
         * ADMINISTRATOR ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/administrators/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of administrators.")]
        PaginatedResponse<User> GetAdministrators(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/administrators/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an administrator.")]
        User GetAdministrator(string id, string token, string fields);

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
        PaginatedResponse<Operator> GetOperators(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/operators/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an operator.")]
        Operator GetOperator(string id, string token, string fields);

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
        PaginatedResponse<Student> GetStudents(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/students/{username}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a student.")]
        Student GetStudent(string username, string token, string fields);

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
        PaginatedResponse<User> GetProfessors(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/professors/{username}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a professor.")]
        User GetProfessor(string username, string token, string fields);

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

        /*
         * COURSES ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/courses/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of courses.")]
        PaginatedResponse<Course> GetCourses(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/courses/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a course.")]
        Course GetCourse(string id, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/courses/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a course.")]
        Course CreateCourse(CourseRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/courses/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a course.")]
        Course UpdateCourse(string id, CourseRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/courses/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a course.")]
        void DeleteCourse(string id, BaseRequest request);

        /*
         * GROUPS ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/groups/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of groups.")]
        PaginatedResponse<Group> GetGroups(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/groups/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a group.")]
        Group GetGroup(string id, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/groups/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a group.")]
        Group CreateGroup(GroupRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/groups/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a group.")]
        Group UpdateGroup(string id, GroupRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/groups/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a group.")]
        void DeleteGroup(string id, BaseRequest request);

        [OperationContract]
        [WebGet(UriTemplate = "/groups/{id}/students/?access_token={token}&q={query}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve the list of students of a group.")]
        List<Student> GetGroupStudents(string id, string token, string query, string sort, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/groups/{id}/students/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Add a list of students to a group.")]
        void AddStudentsToGroup(string id, StudentByGroupRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/groups/{id}/students/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update the list of students of a group.")]
        void UpdateGroupStudents(string id, StudentByGroupRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/groups/{id}/students/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a list of students from a group.")]
        void DeleteStudentsFromGroup(string id, StudentByGroupRequest request);

        /*
         * STUDENT GROUPS ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/students/{username}/groups/?access_token={token}&q={query}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve the list of groups of a student.")]
        List<Group> GetStudentGroups(string username, string token, string query, string sort, string fields);

        /*
         * SOFTWARE ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/software/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of software.")]
        PaginatedResponse<Software> GetSoftwares(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/software/{code}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a software.")]
        Software GetSoftware(string code, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/software/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a software.")]
        Software CreateSoftware(SoftwareRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/software/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a software.")]
        Software UpdateSoftware(string id, SoftwareRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/software/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a software.")]
        void DeleteSoftware(string id, BaseRequest request);

        /*
         * LABORATORY ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/laboratories/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of laboratories.")]
        PaginatedResponse<Laboratory> GetLaboratories(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/laboratories/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a laboratory.")]
        Laboratory GetLaboratory(string id, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/laboratories/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a laboratory.")]
        Laboratory CreateLaboratory(LaboratoryRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/laboratories/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a laboratory.")]
        Laboratory UpdateLaboratory(string id, LaboratoryRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/laboratories/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a laboratory.")]
        void DeleteLaboratory(string id, BaseRequest request);

        [OperationContract]
        [WebGet(UriTemplate = "/laboratories/{id}/software/?access_token={token}&q={query}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve the list of software of a laboratory.")]
        List<Software> GetLaboratorySoftware(string id, string token, string query, string sort, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/laboratories/{id}/software/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Add a list of software to a laboratory.")]
        void AddSoftwareToLaboratory(string id, SoftwareByLaboratoryRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/laboratories/{id}/software/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update the list of software of a laboratory.")]
        void UpdateLaboratorySoftware(string id, SoftwareByLaboratoryRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/laboratories/{id}/software/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a list of software from a laboratory.")]
        void DeleteSoftwareFromLaboratory(string id, SoftwareByLaboratoryRequest request);

        /*
         * APPOINTMENT ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/appointments/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of appointments.")]
        PaginatedResponse<Appointment> GetAppointments(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/appointments/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an appointment.")]
        Appointment GetAppointment(string id, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/appointments/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create an appointment.")]
        Appointment CreateAppointment(AppointmentRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/appointments/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update an appointment.")]
        Appointment UpdateAppointment(string id, AppointmentRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/appointments/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete an appointment.")]
        void DeleteAppointment(string id, BaseRequest request);

        /*
         * STUDENT APPOINTMENTS ENDPOINTS
         */

        [OperationContract]
        [WebGet(UriTemplate = "/students/{username}/appointments/available/?access_token={token}&q={query}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of available appointments dates for a certain student.")]
        List<AvailableAppointment> GetAvailableAppointmentsForCreate(string token, string username, string query, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/students/{username}/appointments/{id}/available/?access_token={token}&q={query}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of available appointment update dates for a certain appointment.")]
        List<AvailableAppointment> GetAvailableAppointmentsForUpdate(string token, string username, string id, string query, string sort, string fields);


        [OperationContract]
        [WebGet(UriTemplate = "/students/{username}/appointments/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of appointments.")]
        PaginatedResponse<Appointment> GetStudentAppointments(string token, string username, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/students/{username}/appointments/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve an appointment.")]
        Appointment GetStudentAppointment(string id, string username, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/students/{username}/appointments/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create an appointment.")]
        Appointment CreateStudentAppointment(string username, AppointmentRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/students/{username}/appointments/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update an appointment.")]
        Appointment UpdateStudentAppointment(string username, string id, AppointmentRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/students/{username}/appointments/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete an appointment.")]
        void DeleteStudentAppointment(string username, string id, BaseRequest request);

        /*
         * RESERVATION ENDPOINTS.
         */

        [OperationContract]
        [WebGet(UriTemplate = "/reservations/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of reservations.")]
        PaginatedResponse<Reservation> GetReservations(string token, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/reservations/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a reservation.")]
        Reservation GetReservation(string id, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/reservations/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a reservation.")]
        Reservation CreateReservation(ReservationRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/reservations/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a reservation.")]
        Reservation UpdateReservation(string id, ReservationRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/reservations/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a reservation.")]
        void DeleteReservation(string id, BaseRequest request);

        /*
         * PROFESSOR RESERVATIONS ENDPOINTS
         */

        [OperationContract]
        [WebGet(UriTemplate = "/professors/{username}/reservations/?access_token={token}&q={query}&page={page}&limit={limit}&sort={sort}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a list of reservations.")]
        PaginatedResponse<Reservation> GetProfessorReservations(string token, string username, string query, string page, string limit, string sort, string fields);

        [OperationContract]
        [WebGet(UriTemplate = "/professors/{username}/reservations/{id}/?access_token={token}&fields={fields}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json),
        Description("Retrieve a reservation.")]
        Reservation GetProfessorReservation(string id, string username, string token, string fields);

        [OperationContract]
        [WebInvoke(Method = "POST",
            UriTemplate = "/professors/{username}/reservations/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Create a reservation.")]
        Reservation CreateProfessorReservation(string username, ReservationRequest request);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            UriTemplate = "/professors/{username}/reservations/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Update a reservation.")]
        Reservation UpdateProfessorReservation(string username, string id, ReservationRequest request);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            UriTemplate = "/professors/{username}/reservations/{id}/",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json),
        Description("Delete a reservation.")]
        void DeleteProfessorReservation(string username, string id, BaseRequest request);

        /*
         * Report Endpoints
         * */
        [OperationContract]
        [WebGet(UriTemplate = "/reports/appointments/student/{username}/?access_token={token}&startdate={startdate}&enddate={enddate}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAppointmentsByStudentReport(string token, string startdate, string enddate, string username);

        [OperationContract]
        [WebGet(UriTemplate = "/reports/appointments/group/{id}/?access_token={token}&startdate={startdate}&enddate={enddate}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        Stream GetAppointmentsByGroupReport(string token, string startdate, string enddate, string id);

    }
}
