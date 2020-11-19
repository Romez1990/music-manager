using Autofac;
using Core.Configuration;
using Core.CoreEngine;
using Core.FileScanner;
using Core.FileSystem;
using Core.HttpClient;
using Core.Operations.Lyrics;
using Core.Operations.Lyrics.Scraper;
using Core.Operations.Lyrics.Searcher;
using Core.Operations.Operation;
using Core.Operations.Rename;
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
            builder.RegisterType<RenameOperation>().AsSelf();
            builder.RegisterType<LyricsOperation>().AsSelf();
            builder.RegisterType<LyricsSearcher>().As<ILyricsSearcher>();
            builder.RegisterType<LyricsScraper>().As<ILyricsScraper>();
            builder.RegisterType<Scanner>().As<IScanner>();
            builder.RegisterType<FsNodeElementFactory>().As<IFsNodeElementFactory>();
            builder.RegisterType<FsNodeFactory>().As<IFsNodeFactory>();
            builder.RegisterType<FsInfoFactory>().As<IFsInfoFactory>();
            builder.RegisterType<Config>().As<IConfig>();
            builder.RegisterType<Http>().As<IHttp>();
            builder.RegisterType<JsonSerializer>().As<IJsonSerializer>();
        }
    }
}
