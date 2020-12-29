using Core.FileSystem;
using Core.Operations.Lyrics.Exceptions;
using LanguageExt;

namespace Core.Operations.Lyrics
{
    public interface ISongLyricsFiller
    {
        EitherAsync<LyricsException, Unit> FillLyrics(IFileElement file);
    }
}
