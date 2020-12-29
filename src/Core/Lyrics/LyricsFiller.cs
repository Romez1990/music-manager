using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Lyrics.Exceptions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Lyrics
{
    public class LyricsFiller : ILyricsFiller
    {
        public LyricsFiller(ISongLyricsFiller songLyricsFiller)
        {
            _songLyricsFiller = songLyricsFiller;
        }

        private readonly ISongLyricsFiller _songLyricsFiller;

        public ImmutableArray<LyricsException> FillLyrics(IDirectoryElement directory, Mode mode)
        {
            var tasks = PerformHelper(directory, mode)
                .Map(eitherAsync => eitherAsync.ToEither())
                .ToArray();
            Task.WaitAll(tasks);
            var exceptions = tasks
                .Map(task => task.Result)
                .Filter(either => either.IsLeft)
                .Map(either => either.IfRight(unit2 => throw new Exception("Unexpected right value")))
                .ToImmutableArray();
            return exceptions;
        }

        private IEnumerable<EitherAsync<LyricsException, Unit>> PerformHelper(IDirectoryElement parentDirectory,
            Mode mode) =>
            mode switch
            {
                Mode.Album => FillAlbum(parentDirectory),
                _ => parentDirectory.Content.Map(fsNode => fsNode switch
                    {
                        IFileElement => Enumerable.Empty<EitherAsync<LyricsException, Unit>>(),
                        IDirectoryElement directory => fsNode.CheckState != CheckState.Unchecked
                            ? PerformHelper(directory, DecreaseMode(mode))
                            : Enumerable.Empty<EitherAsync<LyricsException, Unit>>(),
                        _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                    })
                    .Flatten(),
            };

        private Mode DecreaseMode(Mode mode)
        {
            var modeNumber = (int)mode;
            var newNumber = modeNumber - 1;
            return (Mode)newNumber;
        }

        private IEnumerable<EitherAsync<LyricsException, Unit>> FillAlbum(IDirectoryElement albumDirectory) =>
            albumDirectory.Content.Map(fsNode =>
                fsNode switch
                {
                    IDirectoryElement => unit.AsTask(),
                    IFileElement file => fsNode.CheckState == CheckState.Checked
                        ? _songLyricsFiller.FillLyrics(file)
                        : unit.AsTask(),
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                });
    }
}
