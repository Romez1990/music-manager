using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.FileSystemElement;
using Core.IocContainer;
using Core.OperationEngine;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Lyrics {
    [Service]
    public class LyricsService : ILyricsService {
        public LyricsService(ITrackLyricsService trackLyricsService) {
            _trackLyricsService = trackLyricsService;
        }

        private readonly ITrackLyricsService _trackLyricsService;

        public async Task<IReadOnlyList<LyricsException>> AddLyrics(IDirectoryElement directory,
            DirectoryMode directoryMode) {
            var tasks = PerformHelper(directory, directoryMode)
                .Map(eitherAsync => eitherAsync.ToEither());
            var results = await Task.WhenAll(tasks);
            var exceptions = results
                .Filter(either => either.IsLeft)
                .Map(either => either.IfRight(_ => throw new Exception()))
                .ToArray();
            return exceptions;
        }

        private IEnumerable<EitherAsync<LyricsException, Unit>> PerformHelper(IDirectoryElement parentDirectory,
            DirectoryMode directoryMode) =>
            directoryMode switch
            {
                DirectoryMode.Album => FillAlbum(parentDirectory),
                _ => parentDirectory.Children.Map(fsNode => fsNode switch
                    {
                        IFileElement => Enumerable.Empty<EitherAsync<LyricsException, Unit>>(),
                        IDirectoryElement directory => fsNode.CheckState != CheckState.Unchecked
                            ? PerformHelper(directory, directoryMode.Decrease())
                            : Enumerable.Empty<EitherAsync<LyricsException, Unit>>(),
                        _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                    })
                    .Flatten(),
            };

        private IEnumerable<EitherAsync<LyricsException, Unit>> FillAlbum(IDirectoryElement albumDirectory) =>
            albumDirectory.Children.Map(fsNode =>
                fsNode.Match(
                    _ => unit.AsTask(),
                    file => fsNode.CheckState == CheckState.Checked
                        ? _trackLyricsService.AddLyrics(file)
                        : unit.AsTask()
                ));
    }
}
