using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Reporting.WinForms;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Data;
using System.Data;
using System.ServiceModel.Web;
using SiLabI.Exceptions;
using SiLabI.Util;
using System.Globalization;


namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        /// <summary>
        /// Creates a new appointments by student report.
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="startDate">The start date for the period</param>
        /// <param name="endDate">The end date for the period</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetAppointmentsByStudentReport(string token, string username, string startDate, string endDate)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Administrator);

            StudentDataAccess studentDA = new StudentDataAccess();
            AppointmentDataAccess appointmentDA = new AppointmentDataAccess();

            QueryString studentRequest = new QueryString(ValidFields.Student);
            studentRequest.AccessToken = token;
            studentRequest.AddField("full_name");

            QueryString appointmentRequest = new QueryString(ValidFields.Appointment);
            appointmentRequest.AccessToken = token;
            appointmentRequest.AddQuery("student.username", Relationship.EQ, username);
            appointmentRequest.AddField("state");
            appointmentRequest.AddField("date");
            appointmentRequest.AddField("laboratory.name");
            appointmentRequest.AddField("software.code");
            appointmentRequest.Limit = -1;

            if (startDate != null)
            {
                appointmentRequest.AddQuery("date", Relationship.GE, startDate);
            }

            if (endDate != null)
            {
                appointmentRequest.AddQuery("date", Relationship.LE, endDate);
            }

            DataRow student = studentDA.GetOne(payload, username, studentRequest);
            DataTable appointments = appointmentDA.GetAll(payload, appointmentRequest);
            DateTimeOffset localDate = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-6, 0, 0));

            ReportParameter[] parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("startDate", startDate);
            parameters[1] = new ReportParameter("endDate", endDate);
            parameters[2] = new ReportParameter("studentName", Converter.ToString(student["full_name"]));
            parameters[3] = new ReportParameter("reportDate", Converter.ToString(localDate));
            parameters[4] = new ReportParameter("studentId", username);
  
            string fileName = "Citas_" + username + "." + "pdf";

            return GetReportStream("SiLabI.Reports.AppointmentsByStudentReport.rdlc", appointments, parameters, fileName);
        }

        /// <summary>
        /// Creates a new appointments by group report.
        /// </summary>
        /// <param name="token">The access token</param>
        /// <param name="startDate">The start date for the period</param>
        /// <param name="endDate">The end date for de period</param>
        /// <param name="groupId">The group identifier number</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetAppointmentsByGroupReport(string token, string groupId, string startDate, string endDate)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Administrator);

            int num;
            if (!Int32.TryParse(groupId, out num))
            {
                throw new InvalidParameterException("id");
            }

            GroupDataAccess groupDA = new GroupDataAccess();
            AppointmentDataAccess appointmentDA = new AppointmentDataAccess();

            QueryString groupRequest = new QueryString(ValidFields.Group);
            groupRequest.AccessToken = token;
            groupRequest.AddField("course.code");
            groupRequest.AddField("course.name");
            groupRequest.AddField("number");

            QueryString appointmentRequest = new QueryString(ValidFields.Appointment);
            appointmentRequest.AccessToken = token;
            appointmentRequest.AddQuery("group.id", Relationship.EQ, groupId);
            appointmentRequest.AddField("state");
            appointmentRequest.AddField("date");
            appointmentRequest.AddField("software.code");
            appointmentRequest.AddField("student.username");
            appointmentRequest.Limit = -1;

            if (startDate != null)
            {
                appointmentRequest.AddQuery("date", Relationship.GE, startDate);
            }

            if (endDate != null)
            {
                appointmentRequest.AddQuery("date", Relationship.LE, endDate);
            }

            DataRow group = groupDA.GetOne(payload, num, groupRequest);
            DataTable appointments = appointmentDA.GetAll(payload, appointmentRequest);
            DateTimeOffset localDate = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-6, 0, 0));

            ReportParameter[] parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("startDate", startDate);
            parameters[1] = new ReportParameter("endDate", endDate);
            parameters[2] = new ReportParameter("groupNumber", Converter.ToString(group["number"]));
            parameters[3] = new ReportParameter("courseName", Converter.ToString(group["course.name"]));
            parameters[4] = new ReportParameter("reportDate", Converter.ToString(localDate));

            string fileName = "Citas_Grupo_" + Converter.ToString(group["course.code"]) + "_#" + Converter.ToString(group["number"]) + "." + "pdf";

            return GetReportStream("SiLabI.Reports.AppointmentsByGroupReport.rdlc", appointments, parameters, fileName);
        }

        /// <summary>
        /// Creates a new reservations by professor report.
        /// </summary>
        /// <param name="token"> The session token </param>
        /// <param name="startDate">The start date for the period</param>
        /// <param name="endDate">The end date for de period</param>
        /// <param name="username">The professor username</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetReservationsByProfessorReport(string token, string username, string startDate, string endDate)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Administrator);

            ProfessorDataAccess professorDA = new ProfessorDataAccess();
            ReservationDataAccess reservationDA = new ReservationDataAccess();

            QueryString professorRequest = new QueryString(ValidFields.Professor);
            professorRequest.AccessToken = token;
            professorRequest.AddField("full_name");

            QueryString appointmentRequest = new QueryString(ValidFields.Reservation);
            appointmentRequest.AccessToken = token;
            appointmentRequest.AddQuery("professor.username", Relationship.EQ, username);
            appointmentRequest.AddField("state");
            appointmentRequest.AddField("start_time");
            appointmentRequest.AddField("laboratory.name");
            appointmentRequest.AddField("software.code");
            appointmentRequest.Limit = -1;

            if (startDate != null)
            {
                appointmentRequest.AddQuery("start_time", Relationship.GE, startDate);
            }

            if (endDate != null)
            {
                appointmentRequest.AddQuery("end_time", Relationship.LE, endDate);
            }

            DataRow professor = professorDA.GetOne(payload, username, professorRequest);
            DataTable reservations = reservationDA.GetAll(payload, appointmentRequest);
            DateTimeOffset localDate = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-6, 0, 0));

            ReportParameter[] parameters = new ReportParameter[4];
            parameters[0] = new ReportParameter("startDate", startDate);
            parameters[1] = new ReportParameter("endDate", endDate);
            parameters[2] = new ReportParameter("professor", Converter.ToString(professor["full_name"]));
            parameters[3] = new ReportParameter("reportDate", Converter.ToString(localDate));

            string fileName = "Reservaciones_" + username + "." + "pdf";

            return GetReportStream("SiLabI.Reports.ReservationsByProfessorReport.rdlc", reservations, parameters, fileName);
        }

        /// <summary>
        /// Creates a new reservations by group report.
        /// </summary>
        /// <param name="token"> The session token </param>
        /// <param name="startDate">The start date for the period</param>
        /// <param name="endDate">The end date for de period</param>
        /// <param name="groupId">The group identifier number</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetReservationsByGroupReport(string token, string groupId, string startDate, string endDate)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Administrator);

            int num;
            if (!Int32.TryParse(groupId, out num))
            {
                throw new InvalidParameterException("id");
            }

            GroupDataAccess groupDA = new GroupDataAccess();
            ReservationDataAccess reservationDA = new ReservationDataAccess();

            QueryString groupRequest = new QueryString(ValidFields.Group);
            groupRequest.AccessToken = token;
            groupRequest.AddField("course.code");
            groupRequest.AddField("course.name");
            groupRequest.AddField("number");

            QueryString reservationRequest = new QueryString(ValidFields.Reservation);
            reservationRequest.AccessToken = token;
            reservationRequest.AddQuery("group.id", Relationship.EQ, groupId);
            reservationRequest.AddField("state");
            reservationRequest.AddField("start_time");
            reservationRequest.AddField("software.code");
            reservationRequest.AddField("laboratory.name");
            reservationRequest.Limit = -1;

            if (startDate != null)
            {
                reservationRequest.AddQuery("start_time", Relationship.GE, startDate);
            }

            if (endDate != null)
            {
                reservationRequest.AddQuery("end_time", Relationship.LE, endDate);
            }

            DataRow group = groupDA.GetOne(payload, num, groupRequest);
            DataTable reservations = reservationDA.GetAll(payload, reservationRequest);
            DateTimeOffset localDate = DateTimeOffset.UtcNow.ToOffset(new TimeSpan(-6, 0, 0));

            ReportParameter[] parameters = new ReportParameter[5];
            parameters[0] = new ReportParameter("startDate", startDate);
            parameters[1] = new ReportParameter("endDate", endDate);
            parameters[2] = new ReportParameter("groupNumber", Converter.ToString(group["number"]));
            parameters[3] = new ReportParameter("courseName", Converter.ToString(group["course.name"]));
            parameters[4] = new ReportParameter("courseName", Converter.ToString(localDate));

            string fileName = "Reservaciones_Grupo_" + Converter.ToString(group["course.code"]) + "_#" + Converter.ToString(group["number"]) + "." + "pdf";

            return GetReportStream("SiLabI.Reports.ReservationsByGroupReport.rdlc", reservations, parameters, fileName);
        }

        /// <summary>
        /// Create a report stream.
        /// </summary>
        /// <param name="report">The report resource.</param>
        /// <param name="table">The table data.</param>
        /// <param name="parameters">The report parameters.</param>
        /// <param name="fileName">The name of the output file.</param>
        /// <returns>The stream.</returns>
        private Stream GetReportStream(string report, DataTable table, ReportParameter[] parameters, string fileName)
        {
            ReportDataSource dataSource = new ReportDataSource("DataSet1", table);
            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.ReportEmbeddedResource = report;
            viewer.LocalReport.SetParameters(parameters);
            viewer.LocalReport.DataSources.Add(dataSource);

            OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
            response.ContentType = "application/pdf";
            response.Headers.Add("content-disposition", "attachment; filename=" + fileName);

            byte[] bytes = viewer.LocalReport.Render("PDF");
            MemoryStream ms = new MemoryStream(bytes);

            return ms;
        }
    }
}