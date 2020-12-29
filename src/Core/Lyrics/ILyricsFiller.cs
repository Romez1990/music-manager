using System.Collections.Immutable;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Lyrics.Exceptions;

namespace Core.Lyrics
{
    public interface ILyricsFiller
    {
        ImmutableArray<LyricsException> FillLyrics(IDirectoryElement directory, Mode mode);
    }
}
