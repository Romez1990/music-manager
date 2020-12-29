using Autofac;
using Core.Configuration;
using Core.CoreEngine;
using Core.FileScanner;
using Core.FileSystem;
using Core.HttpClient;
using Core.Lyrics;
using Core.Lyrics.Scraper;
using Core.Lyrics.Searcher;
using Core.Operations;
using Core.Renaming;
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
            builder.RegisterType<Renamer>().As<IRenamer>();
            builder.RegisterType<AlbumRenamer>().As<IAlbumRenamer>();
            builder.RegisterType<TrackRenamer>().As<ITrackRenamer>();
            builder.RegisterType<LyricsFiller>().As<ILyricsFiller>();
            builder.RegisterType<SongLyricsFiller>().As<ISongLyricsFiller>();
            builder.RegisterType<SongFactory>().As<ISongFactory>();
            builder.RegisterType<LyricsSearcher>().As<ILyricsSearcher>();
            builder.RegisterType<LyricsScraper>().As<ILyricsScraper>();
            builder.RegisterType<Scanner>().As<IScanner>();
            builder.RegisterType<FsNodeElementFactory>().As<IFsNodeElementFactory>();
            builder.RegisterType<FsNodeFactory>().As<IFsNodeFactory>();
            builder.RegisterType<FsInfoFactory>().As<IFsInfoFactory>();
            builder.RegisterType<Config>().As<IConfig>();
            builder.RegisterType<ConfigDriver>().As<IConfigDriver>();
            builder.RegisterType<Http>().As<IHttp>();
            builder.RegisterType<JsonSerializer>().As<IJsonSerializer>();
        }
    }
}
