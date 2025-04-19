using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace DoctorPatientChat
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string path = Request.Url.AbsolutePath.ToLower();
            if (path == "/" || path == "/default.aspx" || path == "/default")
            {
                Response.Clear();
                Response.Redirect("~/Login.aspx", true);
                Response.End();
            }
        }
    }
}