using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using RedisDemo.SharedLibrary.Data;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RedisDemo.WebFramework
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureAutofac();
        }

        private void ConfigureAutofac()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            RegisterApplicationDependencies(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private void RegisterApplicationDependencies(ContainerBuilder builder)
        {
            builder.Register<IDistributedCache>(c =>
            {
                var redisOptions = new RedisCacheOptions
                {
                    Configuration = "localhost",
                    InstanceName = "redis_demo_"
                };
                return new RedisCache(redisOptions);
            }).SingleInstance();


            builder.RegisterType<DashboardDataService>()
                .As<IDashboardDataService>()
                .InstancePerLifetimeScope();
        }
    }
}
