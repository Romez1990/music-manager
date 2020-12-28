using System;
using System.ComponentModel;
using Core.CoreEngine;
using Core.FileSystem;

namespace Core.FileScanner
{
    public class Scanner : IScanner
    {
        public IDirectoryElement Scan(IDirectoryElement directory, Mode mode) =>
            mode switch
            {
                Mode.Compilation => ScanCompilationOrBand(directory, mode),
                Mode.Band => ScanCompilationOrBand(directory, mode),
                Mode.Album => ScanAlbum(directory),
                _ => throw new InvalidEnumArgumentException(nameof(mode), (int)mode, typeof(Mode)),
            };

        private IDirectoryElement ScanCompilationOrBand(IDirectoryElement compilationDirectory, Mode mode)
        {
            var nextMode = (Mode)((int)mode - 1);
            return compilationDirectory.MapContent(fsNode =>
                fsNode switch
                {
                    IFileElement => fsNode,
                    IDirectoryElement directory => Scan(directory, nextMode),
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                });
        }

        private IDirectoryElement ScanAlbum(IDirectoryElement directory) =>
            directory.MapContent(fsNode =>
                fsNode switch
                {
                    IDirectoryElement => fsNode,
                    IFileElement file => IsFileToSelect(file)
                        ? file.Check()
                        : fsNode,
                    _ => throw new ArgumentOutOfRangeException(nameof(fsNode)),
                });

        private bool IsFileToSelect(IFileElement file) =>
            file.Extension == ".mp3";
    }
}
