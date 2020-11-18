using System;
using System.Collections.Generic;
using System.Linq;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operations.Lyrics.Exceptions;
using Core.Operations.Operation;
using LanguageExt;
using static LanguageExt.Prelude;

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

        public IDirectoryElement Perform(IDirectoryElement directory, Mode mode)
        {
            var exceptions = PerformHelper(directory, mode)
                .Map(eitherAsync =>
                {
                    var task = eitherAsync.ToEither();
                    task.Wait();
                    return task.Result;
                })
                .Filter(either => either.IsLeft)
                .Map(either => either.IfRight(unit2 => throw new Exception("Unexpected right value")));
            return directory;
        }

        private IEnumerable<EitherAsync<LyricsException, Unit>> PerformHelper(IDirectoryElement parentDirectoryElement,
            Mode mode) =>
            mode switch
            {
                Mode.Album => FillAlbum(parentDirectoryElement),
                _ => parentDirectoryElement.Content.Map(fsNodeElement => fsNodeElement switch
                    {
                        IFileElement _ => Enumerable.Empty<EitherAsync<LyricsException, Unit>>(),
                        IDirectoryElement directory => fsNodeElement.CheckState != CheckState.Unchecked
                            ? PerformHelper(directory, DecreaseMode(mode))
                            : Enumerable.Empty<EitherAsync<LyricsException, Unit>>(),
                        _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                    })
                    .Flatten(),
            };

        private Mode DecreaseMode(Mode mode)
        {
            var modeNumber = (int)mode;
            var newNumber = modeNumber - 1;
            return (Mode)newNumber;
        }

        private IEnumerable<EitherAsync<LyricsException, Unit>> FillAlbum(IDirectoryElement albumDirectoryElement) =>
            albumDirectoryElement.Content.Map(fsNodeElement =>
                fsNodeElement switch
                {
                    IDirectoryElement _ => unit.AsTask(),
                    IFileElement file => fsNodeElement.CheckState == CheckState.Checked
                        ? _lyricsFiller.FillLyrics(file)
                        : unit.AsTask(),
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
    }
}
