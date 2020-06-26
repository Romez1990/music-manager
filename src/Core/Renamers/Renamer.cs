using System;
using System.Linq;
using System.Text.RegularExpressions;
using Core.Actions;
using Core.Engines;
using Core.FileSystem;

namespace Core.Renamers
{
    public class Renamer : IAction
    {
        public void Perform(IDirectoryElement directoryElement, Mode mode)
        {
            switch (mode)
            {
                case Mode.Compilation:
                    RenameCompilation(directoryElement);
                    break;
                case Mode.Band:
                    RenameBand(directoryElement);
                    break;
                case Mode.Album:
                    RenameAlbumDirectorySingle(directoryElement);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        private void RenameCompilation(IDirectoryElement directoryElement)
        {
            directoryElement.Content
                .Where(fsNodeElement => fsNodeElement is IDirectoryElement &&
                                        fsNodeElement.CheckState != CheckState.Unchecked)
                .Cast<IDirectoryElement>()
                .ToList()
                .ForEach(RenameBand);
        }

        private void RenameBand(IDirectoryElement directoryElement)
        {
            directoryElement.Content
                .Where(fsNodeElement => fsNodeElement is IDirectoryElement &&
                                        fsNodeElement.CheckState != CheckState.Unchecked)
                .Cast<IDirectoryElement>()
                .Select((albumDirectoryElement, index) => (albumDirectoryElement, index + 1))
                .ToList()
                .ForEach(tuple =>
                {
                    var (albumDirectoryElement, number) = tuple;
                    RenameAlbumDirectoryInBand(albumDirectoryElement, number);
                });
        }

        private void RenameAlbumDirectoryInBand(IDirectoryElement directoryElement, int number)
        {
            var newName = Regex.Replace(directoryElement.FsNode.Name,
                @"(?<year>\d{4}) - .+ - (?<name>.+)",
                number + " ${name} (${year})");
            directoryElement.FsNode.Rename(newName);
            RenameAlbum(directoryElement);
        }

        private void RenameAlbumDirectorySingle(IDirectoryElement directoryElement)
        {
            var name = directoryElement.FsNode.Name;
            var regex = new Regex(@"(?<year>\d{4}) - .+ - (?<name>.+)");
            if (regex.IsMatch(name))
            {
                const string replacement = "${name} (${year})";
                var newName = regex.Replace(name, replacement);
                directoryElement.FsNode.Rename(newName);
            }

            RenameAlbum(directoryElement);
        }

        private void RenameAlbum(IDirectoryElement directoryElement)
        {
            directoryElement.Content
                .Where(fsNodeElement => fsNodeElement is IFileElement &&
                                        fsNodeElement.CheckState == CheckState.Checked)
                .Cast<IFileElement>()
                .ToList()
                .ForEach(RenameTrack);
        }

        private void RenameTrack(IFileElement fileElement)
        {
            var newName = Regex.Replace(fileElement.FsNode.Name,
                @"(?<number>\d{1,2})\. (?<name>.+\.mp3)",
                "${number} ${name}");
            fileElement.FsNode.Rename(newName);
        }
    }
}
