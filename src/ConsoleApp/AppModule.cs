using Autofac;
using ConsoleApp.ArgumentParser;

namespace ConsoleApp
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OptionsBuilder>().As<IOptionsBuilder>();
        }
    }
}
