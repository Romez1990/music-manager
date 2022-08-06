using System;
using System.Collections.Generic;
using System.Linq;
using Core.FileSystem.Exceptions;
using Core.FileSystemElement;
using Core.IocContainer;
using Core.NamingFormats;
using Core.OperationEngine;
using LanguageExt;
using Utils.Number;
using static LanguageExt.Prelude;

namespace Core.Renamer {
    [Service]
    public class Renamer : IRenamer {
        public Renamer(IAlbumNaming albumNaming, ITrackNaming trackNaming) {
            _albumNaming = albumNaming;
            _trackNaming = trackNaming;
        }

        public RenamingResult Rename(IDirectoryElement directory, DirectoryMode directoryMode) =>
            directoryMode switch {
                DirectoryMode.Compilation => RenameCompilation(directory),
                DirectoryMode.Band => RenameBand(directory),
                DirectoryMode.Album => RenameAlbumSingly(directory),
                _ => throw new NotSupportedException(),
            };

        private RenamingResult RenameCompilation(IDirectoryElement compilationDirectory) {
            var exceptions = new List<FsException>();
            var newDirectory = compilationDirectory.IfChecked(() =>
                compilationDirectory.MapDirectories(bandDirectory => {
                    if (bandDirectory.CheckState is CheckState.Unchecked) return bandDirectory;
                    var renamingResult = RenameBand(bandDirectory);
                    exceptions.AddRange(renamingResult.Exceptions);
                    return renamingResult.Directory;
                }));
            return new RenamingResult(newDirectory, exceptions);
        }

        private RenamingResult RenameBand(IDirectoryElement bandDirectory) {
            var albumNumberLength = bandDirectory.Children.Count.GetLength();
            var exceptions = new List<FsException>();
            var newDirectory = bandDirectory.MapDirectories((albumIndex, albumDirectory) => {
                if (albumDirectory.CheckState is CheckState.Unchecked) return albumDirectory;
                var renamingResult = RenameAlbumInsideBand(albumDirectory, albumIndex + 1, albumNumberLength);
                exceptions.AddRange(renamingResult.Exceptions);
                return renamingResult.Directory;
            });
            return new RenamingResult(newDirectory, exceptions);
        }

        private readonly IAlbumNaming _albumNaming;

        private RenamingResult RenameAlbumInsideBand(IDirectoryElement albumDirectory, int albumNumber,
            int albumNumberLength) =>
            RenameAlbum(albumDirectory,
                _albumNaming.NormalizeWithNumber(albumDirectory.Name, albumNumber, albumNumberLength));

        private RenamingResult RenameAlbumSingly(IDirectoryElement albumDirectory) =>
            RenameAlbum(albumDirectory, _albumNaming.NormalizeWithoutNumber(albumDirectory.Name));

        private RenamingResult RenameAlbum(IDirectoryElement albumDirectory, Option<string> newName) {
            var exceptions = new List<FsException>();
            var newAlbumDirectory = newName
                .Match(albumDirectory.Rename, Right(albumDirectory))
                .IfLeft(exception => {
                    exceptions.Add(exception);
                    return albumDirectory;
                });
            var (newAlbumDirectoryWithChildren, childrenException) = RenameTracks(newAlbumDirectory);
            exceptions.AddRange(childrenException);
            return new RenamingResult(newAlbumDirectoryWithChildren, exceptions);
        }

        private RenamingResult RenameTracks(IDirectoryElement albumDirectory) {
            var checkedChildren = albumDirectory.Children
                .Filter(fsNode => fsNode.CheckState is not CheckState.Unchecked)
                .ToArray();
            var trackNumberLength = checkedChildren.Length.GetLength();
            var prefixLength = _trackNaming.FindPrefixLength(checkedChildren.Map(track => track.Name));
            var exceptions = new List<FsException>();
            var newAlbumDirectory = albumDirectory.MapFiles((trackIndex, track) =>
                track.IfChecked(() =>
                    RenameTrack(track, trackIndex + 1, trackNumberLength, prefixLength)
                        .IfLeft(exception => {
                            exceptions.Add(exception);
                            return track;
                        })));
            return new RenamingResult(newAlbumDirectory, exceptions);
        }

        private readonly ITrackNaming _trackNaming;

        private Either<FsException, IFileElement> RenameTrack(IFileElement track, int trackNumber,
            int trackNumberLength, int prefixLength) =>
            _trackNaming.Normalize(track.Name, trackNumber, trackNumberLength, prefixLength)
                .Match(track.Rename, Right(track));
    }
}
