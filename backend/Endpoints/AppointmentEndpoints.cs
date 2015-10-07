using SiLabI.Exceptions;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        public PaginatedResponse<Appointment> GetAppointments(string token, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            QueryString request = new QueryString(ValidFields.Appointment);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _AppointmentController.GetAll(request, payload);
        }

        public Appointment GetAppointment(string id, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Appointment);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _AppointmentController.GetOne(num, request, payload);
        }

        public Appointment CreateAppointment(AppointmentRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            return _AppointmentController.Create(request, payload);
        }

        public Appointment UpdateAppointment(string id, AppointmentRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _AppointmentController.Update(num, request, payload);
        }

        public void DeleteAppointment(string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Operator);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _AppointmentController.Delete(num, request, payload);
        }

        public List<AvailableAppointment> GetAvailableAppointments(string token, string username, string query, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Student);
            QueryString request = new QueryString(ValidFields.AvailableAppointment);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _AppointmentController.GetAvailable(username, request, payload);
        }

        public PaginatedResponse<Appointment> GetStudentAppointments(string token, string username, string query, string page, string limit, string sort, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Student);

            QueryString request = new QueryString(ValidFields.Appointment);

            request.AccessToken = token;
            request.ParseQuery(query);
            request.ParsePage(page);
            request.ParseLimit(limit);
            request.ParseSort(sort);
            request.ParseFields(fields);

            return _StudentAppointmentController.GetAll(username, request, payload);
        }

        public Appointment GetStudentAppointment(string id, string username, string token, string fields)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Student);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }

            QueryString request = new QueryString(ValidFields.Appointment);

            request.AccessToken = token;
            request.ParseFields(fields);

            return _AppointmentController.GetOne(num, request, payload);
        }

        public Appointment CreateStudentAppointment(string username, AppointmentRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Student);

            return _StudentAppointmentController.Create(username, request, payload);
        }

        public Appointment UpdateStudentAppointment(string username, string id, AppointmentRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Student);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            return _StudentAppointmentController.Update(num, request, payload);
        }

        public void DeleteStudentAppointment(string username, string id, BaseRequest request)
        {
            Dictionary<string, object> payload = Token.Decode(request.AccessToken);
            Token.CheckPayload(payload, UserType.Student);

            int num;
            if (!Int32.TryParse(id, out num))
            {
                throw new InvalidParameterException("id");
            }
            _AppointmentController.Delete(num, request, payload);
        }
    }
}