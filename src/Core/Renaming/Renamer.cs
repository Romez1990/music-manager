using System;
using System.Linq;
using Core.CoreEngine;
using Core.FileSystem;

namespace Core.Renaming
{
    public class Renamer : IRenamer
    {
        public Renamer(IAlbumRenamer albumRenamer, ITrackRenamer trackRenamer)
        {
            _albumRenamer = albumRenamer;
            _trackRenamer = trackRenamer;
        }

        private readonly IAlbumRenamer _albumRenamer;
        private readonly ITrackRenamer _trackRenamer;

        public IDirectoryElement Rename(IDirectoryElement directory, Mode mode)
        {
            var resultDirectory = mode switch
            {
                Mode.Compilation => RenameCompilation(directory),
                Mode.Band => RenameBand(directory),
                Mode.Album => RenameAlbumSingly(directory),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
            return resultDirectory;
        }

        private IDirectoryElement RenameCompilation(IDirectoryElement compilationDirectoryElement) =>
            compilationDirectoryElement.MapContent(fsNode =>
                fsNode switch
                {
                    IFileElement => fsNode,
                    IDirectoryElement directory => fsNode.CheckState != CheckState.Unchecked
                        ? RenameBand(directory)
                        : fsNode,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                });

        private IDirectoryElement RenameBand(IDirectoryElement bandDirectory) =>
            bandDirectory.MapContent((index, fsNode) =>
                fsNode switch
                {
                    IFileElement => fsNode,
                    IDirectoryElement directory => fsNode.CheckState != CheckState.Unchecked
                        ? RenameAlbumInsideBand(directory, index + 1)
                        : fsNode,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                });

        private IDirectoryElement RenameAlbumInsideBand(IDirectoryElement directory, int number)
        {
            var albumsCount = directory
                .Content
                .Count(fsNode => fsNode is IDirectoryElement && fsNode.CheckState == CheckState.Checked);
            var numberLength = albumsCount.ToString().Length;
            var directoryName = _albumRenamer.RenameAlbumWithNumber(directory.Name, numberLength, number);
            var renamedDirectory = directory.Rename(directoryName);
            return RenameAlbumContent(renamedDirectory);
        }

        private IDirectoryElement RenameAlbumSingly(IDirectoryElement directory)
        {
            var directoryName = _albumRenamer.RenameAlbumWithoutNumber(directory.Name);
            var renamedDirectoryElement = directory.Rename(directoryName);
            return RenameAlbumContent(renamedDirectoryElement);
        }

        private IDirectoryElement RenameAlbumContent(IDirectoryElement directory)
        {
            var tracksCount = directory
                .Content
                .Count(fsNodeElement => fsNodeElement.CheckState == CheckState.Checked);
            var isTrackNumberOneDigit = tracksCount < 10;
            return directory.MapContent(fsNodeElement =>
                fsNodeElement switch
                {
                    IDirectoryElement => fsNodeElement,
                    IFileElement file => fsNodeElement.CheckState == CheckState.Checked
                        ? RenameTrack(file, isTrackNumberOneDigit)
                        : fsNodeElement,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private IFileElement RenameTrack(IFileElement file, bool isTrackNumberOneDigit)
        {
            var fileName = _trackRenamer.RenameTrack(file.Name, isTrackNumberOneDigit);
            return file.Rename(fileName);
        }
    }
}
