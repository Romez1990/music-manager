using Autofac;
using Core.Engines;

namespace ConsoleApp.Application
{
    public class AppContainerBuilder
    {
        public IContainer BuildContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AppModule>();
            containerBuilder.RegisterModule<EngineModule>();
            return containerBuilder.Build();
        }
    }
}
