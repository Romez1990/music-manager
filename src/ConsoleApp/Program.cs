using Autofac;
using ConsoleApp.Application;

namespace ConsoleApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var containerBuilder = new AppContainerBuilder();
            var container = containerBuilder.BuildContainer();
            var app = container.Resolve<IApp>();
            app.Configure(args);
            return app.Run();
        }
    }
}
