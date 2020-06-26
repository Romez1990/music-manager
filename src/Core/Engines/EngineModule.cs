using Autofac;
using Core.Actions;
using Core.FileSystem;
using Core.Scanners;

namespace Core.Engines
{
    public class EngineModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Engine>().As<IEngine>();
            builder.RegisterType<ActionsMapper>().As<IActionsMapper>();
            builder.RegisterType<Scanner>().As<IScanner>();
            builder.RegisterType<FsNodeElementFactory>().As<IFsNodeElementFactory>();
            builder.RegisterType<FsNodeFactory>().As<IFsNodeFactory>();
            builder.RegisterType<FileSystemInfoFactory>().As<IFileSystemInfoFactory>();
        }
    }
}
