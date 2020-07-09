using Autofac;
using ConsoleApp.ArgumentParser;
using ConsoleApp.FileSystemTree;

namespace ConsoleApp
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArgParser>().As<IArgParser>();
            builder.RegisterType<OptionsBuilder>().As<IOptionsBuilder>();
            builder.RegisterType<FsTreePrinter>().As<IFsTreePrinter>();
        }
    }
}
