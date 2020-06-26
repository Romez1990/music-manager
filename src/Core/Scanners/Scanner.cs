using System;
using System.Linq;
using Core.Engines;
using Core.FileSystem;

namespace Core.Scanners
{
    public class Scanner : IScanner
    {
        public void Scan(IDirectoryElement directoryElement, Mode mode)
        {
            switch (mode)
            {
                case Mode.Compilation:
                case Mode.Band:
                    ScanOther(directoryElement, mode);
                    break;
                case Mode.Album:
                    ScanAlbum(directoryElement);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        private void ScanOther(IDirectoryElement directoryElement, Mode mode)
        {
            var nextMode = (Mode) ((int) mode - 1);
            directoryElement.Content
                .Where(fsNode => fsNode is IDirectoryElement)
                .Cast<IDirectoryElement>()
                .ToList()
                .ForEach(directory => Scan(directory, nextMode));
        }

        private void ScanAlbum(IDirectoryElement directoryElement)
        {
            directoryElement.Content
                .Where(fsNode => fsNode is IFileElement)
                .Cast<IFileElement>()
                .Where(file => file.FsNode.Extension == ".mp3")
                .ToList()
                .ForEach(file => file.Check());
        }
    }
}
