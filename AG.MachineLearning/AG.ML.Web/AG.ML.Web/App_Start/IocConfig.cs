using AG.ML.Web.AutofacModules;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace AG.ML.Web.App_Start
{
    public static class IocConfig
    {
        public static IContainer CreateContainer()
        {
            var builder = new Autofac.ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var module = new ServicesModule();
            builder.RegisterModule(module);

            return builder.Build();
        }
    }
}