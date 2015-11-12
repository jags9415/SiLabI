using System;
using System.IO;
using System.Net.Mime;
using System.Web;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Reporting.WinForms;
using SiLabI.Model;
using SiLabI.Model.Query;
using SiLabI.Data;
using System.Data;
using System.ServiceModel.Web;


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
        /// <returns>An array of bytes that represents the pdf file</returns>
        public void GetAppointmentsByStudentReport(string token, string start_date, string end_date, string username)
        {
            Dictionary<string, object> payload = Token.Decode(token);
            Token.CheckPayload(payload, UserType.Operator);

            QueryString request = new QueryString(ValidFields.Appointment);
            request.AccessToken = token;
            StudentDataAccess studentAccess = new StudentDataAccess();
            AppointmentDataAccess appDataAccess = new AppointmentDataAccess();
            DataRow studentInfo = studentAccess.GetOne(payload, username, request);

            request.AddField("state");
            request.AddField("date");
            request.AddField("laboratory.name");
            request.AddField("software.code");

            DataTable appInfo = appDataAccess.GetAll(payload, request);

            ReportParameter[] param = new ReportParameter[3];
            param[0] = new ReportParameter("startDate", start_date);
            param[1] = new ReportParameter("endDate", end_date);
            param[2] = new ReportParameter("studentName", studentInfo["full_name"].ToString());
            ReportViewer viewer = new ReportViewer();
            viewer.LocalReport.ReportPath = "Reports/AppointmentsByStudentReport.rdlc";
            viewer.LocalReport.SetParameters(param);
            ReportDataSource dataSource = new ReportDataSource("DataSet1", appInfo);
            viewer.LocalReport.DataSources.Add(dataSource);

            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            byte[] result = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            using (MemoryStream ms = new MemoryStream(result, 0 , result.Length, false, true))
            {
                MailMessage msg = new MailMessage();

                ContentType ct = new ContentType()
                {
                    MediaType = MediaTypeNames.Application.Pdf,
                    Name = "citas_estudiante_" + studentInfo["full_name"].ToString() + ".pdf"
                };

                Attachment att = new Attachment(ms, ct);
                msg.Attachments.Add(att);
            }
        }

        public void GetPDF(string token)
        {
    

            byte[] bytes = System.IO.File.ReadAllBytes("test.pdf");
            MemoryStream ms = new MemoryStream(bytes);
            HttpResponse response = HttpContext.Current.Response;
            response.Buffer = true;
            response.Clear();
            response.ContentType = "application/pdf";
            response.AddHeader("content-disposition", "attachment; filename= test" + "." + "pdf");
            response.BinaryWrite(ms.ToArray());

            response.End();
        }
    }
}