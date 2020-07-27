using System;
using System.ComponentModel;
using Core.CoreEngine;
using Core.FileSystem;

namespace Core.FileScanner
{
    public class Scanner : IScanner
    {
        public IDirectoryElement Scan(IDirectoryElement directoryElement, Mode mode)
        {
            return mode switch
            {
                Mode.Compilation => ScanCompilationOrBand(directoryElement, mode),
                Mode.Band => ScanCompilationOrBand(directoryElement, mode),
                Mode.Album => ScanAlbum(directoryElement),
                _ => throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(Mode)),
            };
        }

        private IDirectoryElement ScanCompilationOrBand(IDirectoryElement compilationDirectoryElement, Mode mode)
        {
            var nextMode = (Mode)((int)mode - 1);
            return compilationDirectoryElement.SelectContent(fsNodeElement =>
                fsNodeElement switch
                {
                    IFileElement _ => fsNodeElement,
                    IDirectoryElement directoryElement => Scan(directoryElement, nextMode),
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private IDirectoryElement ScanAlbum(IDirectoryElement directoryElement)
        {
            return directoryElement.SelectContent(fsNodeElement =>
                fsNodeElement switch
                {
                    IDirectoryElement _ => fsNodeElement,
                    IFileElement fileElement => IsFileElementToSelect(fileElement)
                        ? fileElement.Check()
                        : fsNodeElement,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private bool IsFileElementToSelect(IFileElement fileElement)
        {
            return fileElement.Extension == ".mp3";
        }
    }
}
