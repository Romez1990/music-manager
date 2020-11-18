using Core.FileSystem;
using Core.Operations.Lyrics.Exceptions;
using LanguageExt;

namespace Core.Operations.Lyrics.Scraper
{
    public interface ILyricsScraper
    {
        EitherAsync<LyricsNotFoundException, string> Scrap(IFileElement file, string url);
    }
}
