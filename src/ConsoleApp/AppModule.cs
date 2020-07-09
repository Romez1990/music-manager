using Autofac;
using ConsoleApp.Application;
using ConsoleApp.ArgumentParser;
using ConsoleApp.FileSystemTree;

namespace ConsoleApp
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<App>().As<IApp>();
            builder.RegisterType<ArgParser>().As<IArgParser>();
            builder.RegisterType<OptionsBuilder>().As<IOptionsBuilder>();
            builder.RegisterType<FsTreePrinter>().As<IFsTreePrinter>();
            builder.RegisterType<GraphicalApp>().As<IGraphicalApp>();
        }
    }
}
