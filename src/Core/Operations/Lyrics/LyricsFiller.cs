using System.Threading.Tasks;
using Core.FileSystem;
using Core.Operations.Lyrics.Scraper;
using Core.Operations.Lyrics.Searcher;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Operations.Lyrics
{
    public class LyricsFiller : ILyricsFiller
    {
        public LyricsFiller(ISongFactory songFactory, ILyricsSearcher searcher, ILyricsScraper scraper)
        {
            _songFactory = songFactory;
            _searcher = searcher;
            _scraper = scraper;
        }

        private readonly ISongFactory _songFactory;
        private readonly ILyricsSearcher _searcher;
        private readonly ILyricsScraper _scraper;

        public Task<Unit> FillLyrics(IFileElement file)
        {
            var song = _songFactory.CreateSong(file);
            return GetSearchQuery(song)
                .BindAsync(_searcher.GetLyricsLink)
                .Bind(_scraper.Scrap)
                .IfSome(lyrics => WriteLyrics(song, lyrics));
        }

        private Option<string> GetSearchQuery(ISong song) =>
            Array(song.Artist, song.Title).Sequence()
                .Map(array => MakeSearchQuery(array[0], array[1]));

        private string MakeSearchQuery(string artist, string title) =>
            $"{artist} {title}";

        private void WriteLyrics(ISong song, string lyrics)
        {
            song.Lyrics = lyrics;
            song.Save();
        }
    }
}
