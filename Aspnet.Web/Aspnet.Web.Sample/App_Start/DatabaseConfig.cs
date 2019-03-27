using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Aspnet.Web.Common;

namespace Aspnet.Web.Sample
{
    public class DatabaseConfig
    {
        public static void InitConnectionString()
        {
            ConnectionManager.InitConnectionString(ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString);
        }
    }
}