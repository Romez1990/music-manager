using Core.FileSystem;
using Core.Lyrics.Exceptions;
using LanguageExt;

namespace Core.Lyrics.Scraper
{
    public interface ILyricsScraper
    {
        EitherAsync<LyricsNotFoundException, string> Scrap(IFileElement file, string url);
    }
}
