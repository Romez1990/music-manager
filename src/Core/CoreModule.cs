using Autofac;
using Core.FileSystem;

namespace Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FsNodeElementFactory>().As<IFsNodeElementFactory>();
            builder.RegisterType<FsNodeFactory>().As<IFsNodeFactory>();
            builder.RegisterType<FsInfoFactory>().As<IFsInfoFactory>();
        }
    }
}
