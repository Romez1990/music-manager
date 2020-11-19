using LanguageExt;

namespace Core.Operations.Lyrics.Scraper
{
    public interface ILyricsScraper
    {
        OptionAsync<string> Scrap(string url);
    }
}
