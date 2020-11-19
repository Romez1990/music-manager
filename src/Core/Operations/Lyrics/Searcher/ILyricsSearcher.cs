using LanguageExt;

namespace Core.Operations.Lyrics.Searcher
{
    public interface ILyricsSearcher
    {
        OptionAsync<string> GetLyricsLink(string searchQuery);
    }
}
