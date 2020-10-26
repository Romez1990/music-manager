using Autofac;
using ConsoleApp.Application;

namespace ConsoleApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<AppModule>();
            var container = containerBuilder.Build();

            var app = container.Resolve<IApp>();

            return app.Run(args);
        }
    }
}
