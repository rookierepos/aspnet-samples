using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Aspnet.Web.BLL.Abstractions;
using Aspnet.Web.BLL.Services;
using Aspnet.Web.Common;
using Aspnet.Web.DAL.Abstractions;
using Aspnet.Web.DAL.Dapper;
using Autofac;
using Autofac.Integration.Mvc;

namespace Aspnet.Web.Sample
{
    public class AotofacConfig
    {
        public static void AutofacInit()
        {
            var builder = new ContainerBuilder();
            //builder.Register(r => ConnectionManager.GetConnection()).As<IDbConnection>();//.InstancePerRequest();

            string connectionString = ConfigurationManager.ConnectionStrings["MySQL"].ConnectionString;
            builder.Register(r => ConnectionManager.GetConnection(connectionString))
                .As<IDbConnection>().InstancePerRequest();
            
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<UserService>().As<IUserService>();

            builder.RegisterControllers(Assembly.GetAssembly(typeof(AotofacConfig))).PropertiesAutowired();
            builder.RegisterFilterProvider();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}