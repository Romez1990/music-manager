using System;
using Core.FileSystemElement;
using Core.IocContainer;
using Core.OperationEngine;

namespace Core.Scanner {
    [Service]
    public class Scanner : IScanner {
        public IDirectoryElement Scan(IDirectoryElement directory, DirectoryMode directoryMode) =>
            Expand(Check(directory, directoryMode));

        private IDirectoryElement Check(IDirectoryElement directory, DirectoryMode directoryMode) =>
            directoryMode switch {
                DirectoryMode.Compilation or DirectoryMode.Band => CheckDirectory(directory, directoryMode),
                DirectoryMode.Album => CheckAlbum(directory),
                _ => throw new NotSupportedException(),
            };

        private IDirectoryElement CheckDirectory(IDirectoryElement scanningDirectory, DirectoryMode directoryMode) =>
            scanningDirectory.MapDirectories(directory => Check(directory, directoryMode.Decrease()));

        private IDirectoryElement CheckAlbum(IDirectoryElement scanningDirectory) =>
            scanningDirectory.MapFiles(file => IsFileToCheck(file) ? file.Check() : file);

        private bool IsFileToCheck(IFileElement file) =>
            file.Extension is ".mp3";

        private IDirectoryElement Expand(IDirectoryElement directory) {
            var expandedDirectory = directory.Expand();
            return expandedDirectory.CheckState switch {
                CheckState.Unchecked => expandedDirectory,
                _ => expandedDirectory.MapDirectories(ExpandChildDirectory),
            };
        }

        private IDirectoryElement ExpandChildDirectory(IDirectoryElement directory) =>
            directory.CheckState switch {
                CheckState.Unchecked => directory,
                _ => directory.Expand().MapDirectories(ExpandChildDirectory),
            };
    }
}
