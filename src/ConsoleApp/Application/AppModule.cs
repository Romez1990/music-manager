using Autofac;
using ConsoleApp.FileSystemTree;

namespace ConsoleApp.Application
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<App>().As<IApp>();
            builder.RegisterType<OptionsResolver>().As<IOptionsResolver>();
            builder.RegisterType<FsTreePrinter>().As<IFsTreePrinter>();
        }
    }
}
