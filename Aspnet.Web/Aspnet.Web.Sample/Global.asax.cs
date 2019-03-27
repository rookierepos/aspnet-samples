using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Aspnet.Web.Common;
using Aspnet.Web.Sample;

namespace Aspnet.Web.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            DatabaseConfig.InitConnectionString();
        }

        protected void Session_end(object sender, EventArgs e)
        {
            ConnectionManager.ConnectionDispose();
        }
    }
}
