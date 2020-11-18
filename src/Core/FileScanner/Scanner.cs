using System;
using System.ComponentModel;
using Core.CoreEngine;
using Core.FileSystem;

namespace Core.FileScanner
{
    public class Scanner : IScanner
    {
        public IDirectoryElement Scan(IDirectoryElement directory, Mode mode)
        {
            return mode switch
            {
                Mode.Compilation => ScanCompilationOrBand(directory, mode),
                Mode.Band => ScanCompilationOrBand(directory, mode),
                Mode.Album => ScanAlbum(directory),
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
                    IDirectoryElement directory => Scan(directory, nextMode),
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private IDirectoryElement ScanAlbum(IDirectoryElement directory)
        {
            return directory.SelectContent(fsNodeElement =>
                fsNodeElement switch
                {
                    IDirectoryElement _ => fsNodeElement,
                    IFileElement file => IsFileElementToSelect(file)
                        ? file.Check()
                        : fsNodeElement,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNodeElement)),
                });
        }

        private bool IsFileElementToSelect(IFileElement file)
        {
            return file.Extension == ".mp3";
        }
    }
}
