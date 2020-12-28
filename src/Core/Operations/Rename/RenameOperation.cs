using System;
using System.Linq;
using System.Text.RegularExpressions;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operations.Operation;

namespace Core.Operations.Rename
{
    public class RenameOperation : IOperation
    {
        public string Name { get; } = "Rename";

        public string Description { get; } = "Rename tracks and album folders";

        public OperationResult Perform(IDirectoryElement directory, Mode mode)
        {
            var resultDirectory = mode switch
            {
                Mode.Compilation => RenameCompilation(directory),
                Mode.Band => RenameBand(directory),
                Mode.Album => RenameAlbumSingly(directory),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
            return new OperationResult(resultDirectory, Enumerable.Empty<OperationException>());
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
                .Count(fsNode =>
                    fsNode is IDirectoryElement && fsNode.CheckState == CheckState.Checked);
            var numberLength = albumsCount.ToString().Length;
            var renamedDirectory = RenameAlbumDirectoryWithNumber(directory, numberLength, number);
            return RenameAlbumContent(renamedDirectory);
        }

        private IDirectoryElement RenameAlbumSingly(IDirectoryElement directory)
        {
            var renamedDirectoryElement = RenameAlbumDirectoryWithoutNumber(directory);
            return RenameAlbumContent(renamedDirectoryElement);
        }

        private readonly Regex _albumRegex = new(@"(?<year>\d{4}) - (?:.+ - )?(?<name>.+)");

        private IDirectoryElement RenameAlbumDirectoryWithNumber(IDirectoryElement directory, int numberLength,
            int number)
        {
            var numberString = number.ToString().PadLeft(numberLength, '0');
            var name = directory.Name;
            if (!_albumRegex.IsMatch(name))
                return directory;

            var replacement = numberString + " ${name} (${year})";
            var newName = _albumRegex.Replace(name, replacement);
            return directory.Rename(newName);
        }

        private IDirectoryElement RenameAlbumDirectoryWithoutNumber(IDirectoryElement directory)
        {
            var name = directory.Name;
            if (!_albumRegex.IsMatch(name))
                return directory;

            const string replacement = "${name} (${year})";
            var newName = _albumRegex.Replace(name, replacement);
            return directory.Rename(newName);
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
            var newName = Regex.Replace(file.Name,
                @"(?<number>\d{1,2})(?:\.| -)? (?<name>.+\.mp3)",
                "${number} ${name}");
            if (isTrackNumberOneDigit)
                newName = newName.TrimStart('0');
            return file.Rename(newName);
        }
    }
}
