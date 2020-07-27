using System;
using System.Text.RegularExpressions;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operation;

namespace Core.FileRename
{
    public class Rename : IOperation
    {
        public string Name { get; } = "Rename";

        public string Description { get; } = "Rename tracks and folders";

        public IDirectoryElement Perform(IDirectoryElement directoryElement, Mode mode)
        {
            return mode switch
            {
                Mode.Compilation => RenameCompilation(directoryElement),
                Mode.Band => RenameBand(directoryElement),
                Mode.Album => RenameAlbumDirectorySingle(directoryElement),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };
        }

        private IDirectoryElement RenameCompilation(IDirectoryElement compilationDirectoryElement)
        {
            return compilationDirectoryElement.SelectContent(fsNodeElement =>
                fsNodeElement switch
                {
                    IFileElement _ => fsNodeElement,
                    IDirectoryElement directoryElement => fsNodeElement.CheckState != CheckState.Unchecked
                        ? RenameBand(directoryElement)
                        : fsNodeElement,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private IDirectoryElement RenameBand(IDirectoryElement bandDirectoryElement)
        {
            return bandDirectoryElement.SelectContent((fsNodeElement, index) =>
                fsNodeElement switch
                {
                    IFileElement _ => fsNodeElement,
                    IDirectoryElement directoryElement => fsNodeElement.CheckState != CheckState.Unchecked
                        ? RenameAlbumDirectoryInsideBand(directoryElement, index + 1)
                        : fsNodeElement,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private IDirectoryElement RenameAlbumDirectoryInsideBand(IDirectoryElement directoryElement, int number)
        {
            var renamedDirectoryElement = RenameAlbumDirectoryWithNumber(directoryElement, number);
            return RenameAlbum(renamedDirectoryElement);
        }

        private IDirectoryElement RenameAlbumDirectorySingle(IDirectoryElement directoryElement)
        {
            var renamedDirectoryElement = RenameAlbumDirectoryWithoutNumber(directoryElement);
            return RenameAlbum(renamedDirectoryElement);
        }

        private readonly Regex _albumRegex = new Regex(@"(?<year>\d{4}) - (?:.+ - )?(?<name>.+)");

        private IDirectoryElement RenameAlbumDirectoryWithNumber(IDirectoryElement directoryElement, int number)
        {
            var numberString = number.ToString().PadLeft(2, '0');
            var name = directoryElement.Name;
            if (!_albumRegex.IsMatch(name))
                return directoryElement;

            var replacement = numberString + " ${name} (${year})";
            var newName = _albumRegex.Replace(name, replacement);
            return directoryElement.Rename(newName);
        }

        private IDirectoryElement RenameAlbumDirectoryWithoutNumber(IDirectoryElement directoryElement)
        {
            var name = directoryElement.Name;
            if (!_albumRegex.IsMatch(name))
                return directoryElement;

            const string replacement = "${name} (${year})";
            var newName = _albumRegex.Replace(name, replacement);
            return directoryElement.Rename(newName);
        }

        private IDirectoryElement RenameAlbum(IDirectoryElement directoryElement)
        {
            return directoryElement.SelectContent(fsNodeElement =>
                fsNodeElement switch
                {
                    IDirectoryElement _ => fsNodeElement,
                    IFileElement fileElement => fsNodeElement.CheckState == CheckState.Checked
                        ? RenameTrack(fileElement)
                        : fsNodeElement,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private IFileElement RenameTrack(IFileElement fileElement)
        {
            var newName = Regex.Replace(fileElement.Name,
                @"(?<number>\d{1,2})(?:\.| -)? (?<name>.+\.mp3)",
                "${number} ${name}");
            return fileElement.Rename(newName);
        }
    }
}
