using Core.CoreEngine;
using Core.FileSystem;
using Core.Lyrics;
using Core.Operations.Operation;

namespace Core.Operations.Lyrics
{
    public class LyricsOperation : IOperation
    {
        public LyricsOperation(ILyricsFiller lyricsFiller)
        {
            _lyricsFiller = lyricsFiller;
        }

        private readonly ILyricsFiller _lyricsFiller;

        public string Name => "Lyrics";

        public string Description => "Find lyrics for songs and write it to file";

        public OperationResult Perform(IDirectoryElement directory, Mode mode)
        {
            var exceptions = _lyricsFiller.FillLyrics(directory, mode);
            return new OperationResult(directory, exceptions);
        }
    }
}
