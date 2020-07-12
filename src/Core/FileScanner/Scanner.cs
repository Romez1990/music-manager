using System.ComponentModel;
using System.Linq;
using Core.CoreEngine;
using Core.FileSystem;

namespace Core.FileScanner
{
    public class Scanner : IScanner
    {
        public void Scan(IDirectoryElement directoryElement, Mode mode)
        {
            switch (mode)
            {
                case Mode.Compilation:
                case Mode.Band:
                    ScanCompilationOrBand(directoryElement, mode);
                    break;
                case Mode.Album:
                    ScanAlbum(directoryElement);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(Mode));
            }
        }

        private void ScanCompilationOrBand(IDirectoryElement directoryElement, Mode mode)
        {
            var nextMode = (Mode)((int)mode - 1);
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
                .Where(file => file.Extension == ".mp3")
                .ToList()
                .ForEach(file => file.Check());
        }
    }
}
