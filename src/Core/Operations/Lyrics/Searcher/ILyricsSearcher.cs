using Core.FileSystem;
using Core.Operations.Lyrics.Exceptions;
using LanguageExt;

namespace Core.Operations.Lyrics.Searcher
{
    public interface ILyricsSearcher
    {
        EitherAsync<LyricsException, string> GetLyricsLink(IFileElement file, string searchQuery);
    }
}
