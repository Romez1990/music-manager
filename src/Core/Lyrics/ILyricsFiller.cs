using System.Collections.Immutable;
using System.Threading.Tasks;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Lyrics.Exceptions;

namespace Core.Lyrics
{
    public interface ILyricsFiller
    {
        Task<ImmutableArray<LyricsException>> FillLyrics(IDirectoryElement directory, Mode mode);
    }
}
