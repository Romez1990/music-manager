using Autofac;
using Core;

namespace ConsoleApp
{
    public class AppContainerBuilder
    {
        public IContainer Build()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AppModule>();
            containerBuilder.RegisterModule<CoreModule>();
            return containerBuilder.Build();
        }
    }
}
