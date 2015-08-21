using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace SiLabI
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.Add(new ServiceRoute("", new WebServiceHostFactory(), typeof(SiLabI.Service)));
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Uri url = HttpContext.Current.Request.Url;

            if (!url.AbsolutePath.EndsWith("/"))
            {
                HttpContext.Current.RewritePath(url.AbsolutePath + "/" + url.Query);
            }
        }
    }
}