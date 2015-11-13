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
        /// <param name="token"> The session token </param>
        /// <param name="start_date">The start date for the period</param>
        /// <param name="end_date">The end date for de period</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetAppointmentsByStudentReport(string token, string start_date, string end_date, string username)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            QueryString stdrequest = new QueryString(ValidFields.Student);
            stdrequest.AccessToken = token;
            stdrequest.AddField("full_name");

            QueryString request = new QueryString(ValidFields.Appointment);
            request.AccessToken = token;
            StudentDataAccess studentAccess = new StudentDataAccess();
            AppointmentDataAccess appDataAccess = new AppointmentDataAccess();
            DataRow studentInfo = studentAccess.GetOne(payload, username, stdrequest);

            request.AccessToken = token;
            request.AddQuery("student.username", Relationship.EQ, username);
            request.AddField("state");
            request.AddField("date");
            request.AddField("laboratory.name");
            request.AddField("software.code");
            request.Limit = -1;

            if (start_date != null)
            {
                request.AddQuery("date", Relationship.GE, start_date);
            }

            if (end_date != null)
            {
                request.AddQuery("date", Relationship.LE, end_date);
            }

            DataTable appInfo = appDataAccess.GetAll(payload, request);

            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("startDate", start_date);
            param[1] = new ReportParameter("endDate", end_date);
            param[2] = new ReportParameter("studentName", studentInfo["full_name"].ToString());
            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.ReportEmbeddedResource = "SiLabI.Reports.AppointmentsByStudentReport.rdlc";
            viewer.LocalReport.SetParameters(param);
            ReportDataSource dataSource = new ReportDataSource("DataSet1", appInfo);
            viewer.LocalReport.DataSources.Add(dataSource);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            MemoryStream ms = new MemoryStream(bytes);
            OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;

            response.ContentType = "application/pdf";
            response.Headers.Add("content-disposition", "attachment; filename=citas_" + studentInfo["full_name"].ToString() + "." + "pdf");

            return ms;
        }

        /// <summary>
        /// Creates a new appointments by group report.
        /// </summary>
        /// <param name="token"> The session token </param>
        /// <param name="start_date">The start date for the period</param>
        /// <param name="end_date">The end date for de period</param>
        /// <param name="group_id">The group identifier number</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetAppointmentsByGroupReport(string token, string start_date, string end_date, string group_id)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            QueryString group_request = new QueryString(ValidFields.Group);
            group_request.AccessToken = token;
            GroupDataAccess groupAccess = new GroupDataAccess();
            AppointmentDataAccess appDataAccess = new AppointmentDataAccess();
            int num;
            if (!Int32.TryParse(group_id, out num))
            {
                throw new InvalidParameterException("id");
            }

            group_request.AddField("course.name");
            group_request.AddField("number");
            DataRow groupInfo = groupAccess.GetOne(payload, num, group_request);

            QueryString request = new QueryString(ValidFields.Appointment);

            request.AccessToken = token;
            request.AddQuery("group.id", Relationship.EQ, group_id);
            request.AddField("state");
            request.AddField("date");
            request.AddField("software.code");
            request.AddField("student.username");
            request.Limit = -1;

            if (start_date != null)
            {
                request.AddQuery("date", Relationship.GE, start_date);
            }

            if (end_date != null)
            {
                request.AddQuery("date", Relationship.LE, end_date);
            }

            DataTable appInfo = appDataAccess.GetAll(payload, request);

            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("startDate", start_date);
            param[1] = new ReportParameter("endDate", end_date);
            param[2] = new ReportParameter("groupNumber", groupInfo["number"].ToString());
            param[3] = new ReportParameter("courseName", groupInfo["course.name"].ToString());

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.ReportEmbeddedResource = "SiLabI.Reports.AppointmentsByGroupReport.rdlc";
            viewer.LocalReport.SetParameters(param);
            ReportDataSource dataSource = new ReportDataSource("DataSet1", appInfo);
            viewer.LocalReport.DataSources.Add(dataSource);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            MemoryStream ms = new MemoryStream(bytes);
            OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;

            response.ContentType = "application/pdf";
            response.Headers.Add("content-disposition", "attachment; filename=citas_grupo_" + groupInfo["course.name"].ToString() + "_#" + groupInfo["number"].ToString() + "." + "pdf");

            return ms;
        }

        /// <summary>
        /// Creates a new reservations by group report.
        /// </summary>
        /// <param name="token"> The session token </param>
        /// <param name="start_date">The start date for the period</param>
        /// <param name="end_date">The end date for de period</param>
        /// <param name="group_id">The group identifier number</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetReservationsByGroupReport(string token, string start_date, string end_date, string group_id)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            QueryString group_request = new QueryString(ValidFields.Group);
            group_request.AccessToken = token;
            GroupDataAccess groupAccess = new GroupDataAccess();
            

            int num;
            if (!Int32.TryParse(group_id, out num))
            {
                throw new InvalidParameterException("id");
            }

            ReservationDataAccess resDataAccess = new ReservationDataAccess();

            group_request.AddField("course.name");
            group_request.AddField("number");
            DataRow groupInfo = groupAccess.GetOne(payload, num, group_request);

            QueryString request = new QueryString(ValidFields.Reservation);

            request.AccessToken = token;
            request.AddQuery("group.id", Relationship.EQ, group_id);
            request.AddField("state");
            request.AddField("start_time");
            request.AddField("software.code");
            request.AddField("laboratory.name");
            request.Limit = -1;

            if (start_date != null)
            {
                request.AddQuery("start_time", Relationship.GE, start_date);
            }

            if (end_date != null)
            {
                request.AddQuery("end_time", Relationship.LE, end_date);
            }

            DataTable resInfo = resDataAccess.GetAll(payload, request);

            ReportParameter[] param = new ReportParameter[4];
            param[0] = new ReportParameter("startDate", start_date);
            param[1] = new ReportParameter("endDate", end_date);
            param[2] = new ReportParameter("groupNumber", groupInfo["number"].ToString());
            param[3] = new ReportParameter("courseName", groupInfo["course.name"].ToString());

            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.ReportEmbeddedResource = "SiLabI.Reports.ReservationsByGroupReport.rdlc";
            viewer.LocalReport.SetParameters(param);
            ReportDataSource dataSource = new ReportDataSource("DataSet1", resInfo);
            viewer.LocalReport.DataSources.Add(dataSource);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            MemoryStream ms = new MemoryStream(bytes);
            OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;

            response.ContentType = "application/pdf";
            response.Headers.Add("content-disposition", "attachment; filename=reservaciones_grupo_" + groupInfo["course.name"].ToString() + "_#" + groupInfo["number"].ToString() + "." + "pdf");

            return ms;
        }

        /// <summary>
        /// Creates a new reservations by professor report.
        /// </summary>
        /// <param name="token"> The session token </param>
        /// <param name="start_date">The start date for the period</param>
        /// <param name="end_date">The end date for de period</param>
        /// <param name="username">The professor username</param>
        /// <returns>A pdf binary file</returns>
        public Stream GetReservationsByProfessorReport(string token, string start_date, string end_date, string username)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);
            QueryString profrequest = new QueryString(ValidFields.Professor);
            profrequest.AccessToken = token;
            profrequest.AddField("full_name");
            ProfessorDataAccess professorAccess = new ProfessorDataAccess();
            
            DataRow professorInfo = professorAccess.GetOne(payload, username, profrequest);

            QueryString request = new QueryString(ValidFields.Reservation);
            request.AccessToken = token;
            request.AddQuery("professor.username", Relationship.EQ, username);
            request.AddField("state");
            request.AddField("start_time");
            request.AddField("laboratory.name");
            request.AddField("software.code");
            request.Limit = -1;

            if (start_date != null)
            {
                request.AddQuery("start_time", Relationship.GE, start_date);
            }

            if (end_date != null)
            {
                request.AddQuery("end_time", Relationship.LE, end_date);
            }

            ReservationDataAccess resDataAccess = new ReservationDataAccess();
            DataTable resInfo = resDataAccess.GetAll(payload, request);

            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("startDate", start_date);
            param[1] = new ReportParameter("endDate", end_date);
            param[2] = new ReportParameter("professor", professorInfo["full_name"].ToString());
            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.ReportEmbeddedResource = "SiLabI.Reports.ReservationsByProfessorReport.rdlc";
            viewer.LocalReport.SetParameters(param);
            ReportDataSource dataSource = new ReportDataSource("DataSet1", resInfo);
            viewer.LocalReport.DataSources.Add(dataSource);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            MemoryStream ms = new MemoryStream(bytes);
            OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;

            response.ContentType = "application/pdf";
            response.Headers.Add("content-disposition", "attachment; filename=reservaciones_" + professorInfo["full_name"].ToString() + "." + "pdf");

            return ms;
        }

    }
}