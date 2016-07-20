using AG.ML.Web.DataAccess;
using AG.ML.Web.Services;
using Autofac;
using MongoDB.Driver;

namespace AG.ML.Web.AutofacModules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new DatabaseServer(MongoUrl.Create("mongodb://localhost/machinelearning")).Database).AsSelf().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<NetworkService>().As<INetworkService>();
        }
    }
}