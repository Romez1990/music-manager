using Core.CoreEngine;
using Core.FileSystem;
using Core.Lyrics;

namespace Core.Operations
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
            var task = _lyricsFiller.FillLyrics(directory, mode);
            task.Wait();
            var exceptions = task.Result;
            return new OperationResult(directory, exceptions);
        }
    }
}
