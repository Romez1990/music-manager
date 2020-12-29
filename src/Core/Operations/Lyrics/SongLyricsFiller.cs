using Core.FileSystem;
using Core.Operations.Lyrics.Exceptions;
using Core.Operations.Lyrics.Scraper;
using Core.Operations.Lyrics.Searcher;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Operations.Lyrics
{
    public class SongLyricsFiller : ISongLyricsFiller
    {
        public SongLyricsFiller(ISongFactory songFactory, ILyricsSearcher searcher, ILyricsScraper scraper)
        {
            _songFactory = songFactory;
            _searcher = searcher;
            _scraper = scraper;
        }

        private readonly ISongFactory _songFactory;
        private readonly ILyricsSearcher _searcher;
        private readonly ILyricsScraper _scraper;

        public EitherAsync<LyricsException, Unit> FillLyrics(IFileElement file)
        {
            var song = _songFactory.CreateSong(file);
            return GetSearchQuery(file, song).ToAsync()
                .Bind(searchQuery => _searcher.GetLyricsLink(file, searchQuery))
                .Bind(searchQuery => _scraper.Scrap(file, searchQuery)
                    .MapLeft(e => (LyricsException)e))
                .Map(lyrics => WriteLyrics(song, lyrics));
        }

        private Either<LyricsException, string> GetSearchQuery(IFileElement file, ISong song) =>
            Array(song.Artist, song.Title).Sequence()
                .Map(array => MakeSearchQuery(array[0], array[1]))
                .ToEither(() => (LyricsException)new MissingTagsException(file));

        private string MakeSearchQuery(string artist, string title) =>
            $"{artist} {title}";

        private Unit WriteLyrics(ISong song, string lyrics)
        {
            song.Lyrics = lyrics;
            song.Save();
            return unit;
        }
    }
}
