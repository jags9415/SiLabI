using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace SiLabI
{
    /// <summary>
    /// The web service implementation.
    /// </summary>
    public partial class Service
    {
        public Stream GetWelcomeMessage()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("       WELCOME TO                                    ");
            sb.AppendLine("           SILABI                                    ");
            sb.AppendLine("               \\  .-.                                ");
            sb.AppendLine("                 /_ _\\                               ");
            sb.AppendLine("                 |o^o|                               ");
            sb.AppendLine("                 \\ _ /                               ");
            sb.AppendLine("                .-'-'-.                              ");
            sb.AppendLine("              /`)  .  (`\\           BLEEP BOOP BLOOP ");
            sb.AppendLine("             / /|.-'-.|\\ \\         /                 ");
            sb.AppendLine("             \\ \\| (_) |/ /  .-\"\"-.                   ");
            sb.AppendLine("              \\_\'-.-'/_/  /[] _ _\\                   ");
            sb.AppendLine("              /_/ \\_/ \\_\\ _|_o_LII|_                 ");
            sb.AppendLine("                |'._.'|  / | ==== | \\                ");
            sb.AppendLine("                |  |  |  |_| ==== |_|                ");
            sb.AppendLine("                 \\_|_/    ||  ||  ||                 ");
            sb.AppendLine("                 |-|-|    ||LI  o ||                 ");
            sb.AppendLine("                 |_|_|    ||'----'||                 ");
            sb.AppendLine("                /_/ \\_\\  /__|    |__\\                ");

            string result = sb.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(result);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/plain";

            return new MemoryStream(bytes);
        }
    }
}