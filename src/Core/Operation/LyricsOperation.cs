using System.Collections.Immutable;
using Core.FileSystemElement;
using Core.IocContainer;
using Core.Lyrics;
using Core.OperationEngine;

namespace Core.Operation {
    [Service(ToSelf = true)]
    public class LyricsOperation : IOperation {
        public LyricsOperation(ILyricsService lyricsService) {
            _lyricsService = lyricsService;
        }

        private readonly ILyricsService _lyricsService;

        public string Name => "Lyrics";

        public string Description => "Find lyrics for songs and write it to file";

        public OperationResult Perform(IDirectoryElement directory, DirectoryMode directoryMode) {
            var task = _lyricsService.AddLyrics(directory, directoryMode);
            task.Wait();
            var exceptions = task.Result
                .Map(fsException => new OperationException(fsException))
                .ToImmutableList();
            return new OperationResult(directory, exceptions);
        }
    }
}
