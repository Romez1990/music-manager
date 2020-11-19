using Autofac;
using Core.CoreEngine;
using Core.FileScanner;
using Core.FileSystem;
using Core.Operations.Operation;
using Core.Serializers;

namespace Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Engine>().As<IEngine>();
            builder.RegisterType<EngineFactory>().As<IEngineFactory>();
            builder.RegisterType<OperationRepository>().As<IOperationRepository>();
            builder.RegisterType<Scanner>().As<IScanner>();
            builder.RegisterType<FsNodeElementFactory>().As<IFsNodeElementFactory>();
            builder.RegisterType<FsNodeFactory>().As<IFsNodeFactory>();
            builder.RegisterType<FsInfoFactory>().As<IFsInfoFactory>();
            builder.RegisterType<JsonSerializer>().As<IJsonSerializer>();
        }
    }
}
