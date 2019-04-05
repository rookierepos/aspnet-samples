using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Aspnet.Web.Common;
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

            var baseRepositoryType = typeof(DAL.Dapper.BaseRepository);
            var repositories = Assembly.GetAssembly(baseRepositoryType)
                .ExportedTypes.Where(type => type.BaseType == baseRepositoryType);
            foreach (Type repository in repositories)
            {
                builder.RegisterType(repository)
                    .As(repository.GetInterface($"I{repository.Name}"));
            }
            //builder.RegisterType<UserRepository>().As<IUserRepository>();
            //builder.RegisterType<RoleRepository>().As<IRoleRepository>();
            
            var baseServiceType = typeof(BLL.Services.BaseService);
            var services = Assembly.GetAssembly(baseServiceType)
                .ExportedTypes.Where(type => type.BaseType == baseServiceType);
            foreach (var service in services)
            {
                builder.RegisterType(service)
                    .As(service.GetInterface($"I{service.Name}"));
            }
            //builder.RegisterType<AccountService>().As<IAccountService>();

            builder.RegisterControllers(Assembly.GetAssembly(typeof(AotofacConfig))).PropertiesAutowired();
            builder.RegisterFilterProvider();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}