using System;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operations.Operation;

namespace Core.Operations.Lyrics
{
    public class LyricsOperation : IOperation
    {
        public string Name => "Lyrics";

        public string Description => "Find lyrics for songs and write it to file";

        public IDirectoryElement Perform(IDirectoryElement directory, Mode mode)
        {
            throw new NotImplementedException();
        }
    }
}
