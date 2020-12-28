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

        public string Description { get; } = "Rename tracks and folders";

        public OperationResult Perform(IDirectoryElement directory, Mode mode)
        {
            var resultDirectory = mode switch
            {
                Mode.Compilation => RenameCompilation(directory),
                Mode.Band => RenameBand(directory),
                Mode.Album => RenameAlbumDirectorySingle(directory),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
            return new OperationResult(resultDirectory, Enumerable.Empty<OperationException>());
        }

        private IDirectoryElement RenameCompilation(IDirectoryElement compilationDirectoryElement) =>
            compilationDirectoryElement.SelectContent(fsNode =>
                fsNode switch
                {
                    IFileElement _ => fsNode,
                    IDirectoryElement directory => fsNode.CheckState != CheckState.Unchecked
                        ? RenameBand(directory)
                        : fsNode,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                });

        private IDirectoryElement RenameBand(IDirectoryElement bandDirectory) =>
            bandDirectory.SelectContent((fsNode, index) =>
                fsNode switch
                {
                    IFileElement _ => fsNode,
                    IDirectoryElement directory => fsNode.CheckState != CheckState.Unchecked
                        ? RenameAlbumDirectoryInsideBand(directory, index + 1)
                        : fsNode,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                });

        private IDirectoryElement RenameAlbumDirectoryInsideBand(IDirectoryElement directory, int number)
        {
            var albumsCount = directory
                .Content
                .Count(fsNode =>
                    fsNode is IDirectoryElement && fsNode.CheckState == CheckState.Checked);
            var numberLength = albumsCount.ToString().Count();
            var renamedDirectory = RenameAlbumDirectoryWithNumber(directory, numberLength, number);
            return RenameAlbum(renamedDirectory);
        }

        private IDirectoryElement RenameAlbumDirectorySingle(IDirectoryElement directory)
        {
            var renamedDirectoryElement = RenameAlbumDirectoryWithoutNumber(directory);
            return RenameAlbum(renamedDirectoryElement);
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

        private IDirectoryElement RenameAlbum(IDirectoryElement directory)
        {
            var tracksCount = directory
                .Content
                .Count(fsNodeElement => fsNodeElement.CheckState == CheckState.Checked);
            var isTrackNumberOneDigit = tracksCount < 10;
            return directory.SelectContent(fsNodeElement =>
                fsNodeElement switch
                {
                    IDirectoryElement _ => fsNodeElement,
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
