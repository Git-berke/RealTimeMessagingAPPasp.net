using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace DoctorPatientChat
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "DefaultRoute",
                "",
                "~/Login.aspx"
            );

            routes.MapPageRoute(
                "DefaultPageRoute",
                "Default",
                "~/Login.aspx"
            );

            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
        }
    }
}
