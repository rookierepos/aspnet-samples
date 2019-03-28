using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Aspnet.Web.BLL.Abstractions;
using Aspnet.Web.BLL.Services;
using Aspnet.Web.Common;
using Autofac;
using Autofac.Integration.Mvc;
using Microsoft.Ajax.Utilities;

namespace Aspnet.Web.Sample
{
    public class AotofacConfig
    {
        public static void AutofacInit()
        {
            var builder = new ContainerBuilder();
            builder.Register(r => ConnectionManager.GetConnection()).As<IDbConnection>();//.InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>();

            builder.RegisterControllers(Assembly.GetAssembly(typeof(AotofacConfig))).PropertiesAutowired();
            builder.RegisterFilterProvider();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}