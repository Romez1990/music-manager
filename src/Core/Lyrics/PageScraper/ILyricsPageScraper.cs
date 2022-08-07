using Core.Lyrics.PageScraper.Exceptions;
using LanguageExt;

namespace Core.Lyrics.PageScraper {
    public interface ILyricsPageScraper {
        EitherAsync<PageScrapException, string> ScrapLyrics(string pageUrl);
    }
}
