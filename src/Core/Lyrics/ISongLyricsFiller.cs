using Core.FileSystem;
using Core.Lyrics.Exceptions;
using LanguageExt;

namespace Core.Lyrics
{
    public interface ISongLyricsFiller
    {
        EitherAsync<LyricsException, Unit> FillLyrics(IFileElement file);
    }
}
