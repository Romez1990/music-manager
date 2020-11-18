using Core.FileSystem;
using Core.Operations.Lyrics.Exceptions;
using LanguageExt;

namespace Core.Operations.Lyrics
{
    public interface ILyricsFiller
    {
        EitherAsync<LyricsException, Unit> FillLyrics(IFileElement file);
    }
}
