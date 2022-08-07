using System;
using Core.FileSystemElement;
using Core.IocContainer;
using Core.Lyrics.PageScraper;
using Core.Lyrics.PageSearcher;
using Core.Lyrics.TrackWriter;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Lyrics {
    [Service]
    public class TrackLyricsService : ITrackLyricsService {
        public TrackLyricsService(ITrackFactory trackFactory, ILyricsPageSearcher pageSearcher,
            ILyricsPageScraper pageScraper, ITrackLyricsWriter trackWriter) {
            _trackFactory = trackFactory;
            _pageSearcher = pageSearcher;
            _pageScraper = pageScraper;
            _trackWriter = trackWriter;
        }

        private readonly ITrackFactory _trackFactory;
        private readonly ILyricsPageSearcher _pageSearcher;
        private readonly ILyricsPageScraper _pageScraper;
        private readonly ITrackLyricsWriter _trackWriter;

        public EitherAsync<LyricsException, Unit> AddLyrics(IFileElement file) {
            var track = _trackFactory.CreateTrack(file);
            var castException = new Func<LyricsException, LyricsException>(identity);
            return _pageSearcher.SearchPageUrl(track).MapLeft(castException)
                .Bind(pageUrl => _pageScraper.ScrapLyrics(pageUrl).MapLeft(castException))
                .Bind(lyrics => _trackWriter.WriteLyrics(track, lyrics).MapLeft(castException).ToAsync());
        }
    }
}
