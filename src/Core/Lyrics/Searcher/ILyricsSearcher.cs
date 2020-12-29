using Core.FileSystem;
using Core.Lyrics.Exceptions;
using LanguageExt;

namespace Core.Lyrics.Searcher
{
    public interface ILyricsSearcher
    {
        EitherAsync<LyricsException, string> GetLyricsLink(IFileElement file, string searchQuery);
    }
}
