using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Moq;
using Core.FileSystem;
using Directory = System.IO.Directory;

namespace Core.Test.FileSystem
{
    public class MockFsNodeElementFactory : IFsNodeElementFactory
    {
        public MockFsNodeElementFactory()
        {
            _testFilesLoader = new TestFilesLoader();
            _fileSystemRoot = Directory.GetDirectoryRoot(_testFilesLoader.TestFilesDirectoryPath);
            ScanTestFiles();
            CompilationDirectoryElement = CreateDirectoryElement(CompilationPath, ChangeCompilationDirectoryElement);
            BandDirectoryElement = CreateDirectoryElement(BandPath, ChangeBandDirectoryElement);
            AlbumDirectoryElement = CreateDirectoryElement(AlbumPath, ChangeAlbumDirectoryElement);
        }

        private readonly TestFilesLoader _testFilesLoader;

        private readonly Dictionary<string, IFsNode> _fsNodes = new Dictionary<string, IFsNode>();

        private readonly string _fileSystemRoot;

        private string CompilationPath { get; set; }

        private string BandPath { get; set; }

        private string AlbumPath { get; set; }

        public IDirectoryElement CompilationDirectoryElement { get; private set; }

        public IDirectoryElement BandDirectoryElement { get; private set; }

        public IDirectoryElement AlbumDirectoryElement { get; private set; }

        private void ChangeCompilationDirectoryElement(object sender, FsNodeElementCheckEventArgs e)
        {
            CompilationDirectoryElement = (IDirectoryElement)e.FsNodeElement;
        }

        private void ChangeBandDirectoryElement(object sender, FsNodeElementCheckEventArgs e)
        {
            BandDirectoryElement = (IDirectoryElement)e.FsNodeElement;
        }

        private void ChangeAlbumDirectoryElement(object sender, FsNodeElementCheckEventArgs e)
        {
            AlbumDirectoryElement = (IDirectoryElement)e.FsNodeElement;
        }

        private void ScanTestFiles()
        {
            var testFiles = _testFilesLoader.GetFiles();
            ScanCompilation(testFiles);
        }

        private string CutPath(FileSystemInfo info)
        {
            var relativePath = Path.GetRelativePath(_testFilesLoader.TestFilesDirectoryPath, info.FullName);
            return Path.Combine(_fileSystemRoot, relativePath);
        }

        private void ScanCompilation(
            Dictionary<DirectoryInfo, Dictionary<DirectoryInfo, FileInfo[]>> bandDirectoriesInfo)
        {
            BandPath = CutPath(bandDirectoriesInfo.OrderBy(fsNode => fsNode.Key.Name).First().Key);
            CompilationPath = _fileSystemRoot;
            var mockCompilationDirectory = MockCompilation(bandDirectoriesInfo);
            _fsNodes[CompilationPath] = mockCompilationDirectory;
        }

        private IDirectory ScanBand(KeyValuePair<DirectoryInfo, Dictionary<DirectoryInfo, FileInfo[]>> pair)
        {
            var (bandDirectoryInfo, albumDirectoriesInfo) = pair;
            AlbumPath = CutPath(albumDirectoriesInfo.OrderBy(fsNode => fsNode.Key.Name).First().Key);
            var mockBandDirectory = MockBand(bandDirectoryInfo, albumDirectoriesInfo);
            _fsNodes[CutPath(bandDirectoryInfo)] = mockBandDirectory;
            return mockBandDirectory;
        }

        private IDirectory ScanAlbum(KeyValuePair<DirectoryInfo, FileInfo[]> pair)
        {
            var (albumDirectoryInfo, tracksInfo) = pair;
            var mockAlbumDirectory = MockAlbum(albumDirectoryInfo, tracksInfo);
            _fsNodes[CutPath(albumDirectoryInfo)] = mockAlbumDirectory;
            return mockAlbumDirectory;
        }

        private IFile ScanTrack(FileInfo fileInfo)
        {
            var mockTrack = MockTrack(fileInfo);
            _fsNodes[CutPath(fileInfo)] = mockTrack;
            return mockTrack;
        }

        private IDirectory MockCompilation(
            Dictionary<DirectoryInfo, Dictionary<DirectoryInfo, FileInfo[]>> bandDirectoriesInfo)
        {
            var mockCompilationDirectory = new Mock<IDirectory>(MockBehavior.Strict);
            var name = mockCompilationDirectory.Name;
            mockCompilationDirectory.Setup(directory => directory.Name).Returns(() => name);
            mockCompilationDirectory.Setup(directory => directory.Path).Returns(CompilationPath);
            mockCompilationDirectory.Setup(directory => directory.ToString()).Returns(() => name);
            mockCompilationDirectory.Setup(directory => directory.Content).Returns(
                bandDirectoriesInfo
                    .Select(ScanBand)
                    .Cast<IFsNode>()
                    .OrderBy(fsNode => fsNode.Name)
                    .ToImmutableArray()
            );
            return mockCompilationDirectory.Object;
        }

        private IDirectory MockBand(DirectoryInfo bandDirectoryInfo,
            Dictionary<DirectoryInfo, FileInfo[]> albumDirectoriesInfo)
        {
            var mockBandDirectory = new Mock<IDirectory>(MockBehavior.Strict);
            var name = bandDirectoryInfo.Name;
            mockBandDirectory.Setup(directory => directory.Name).Returns(() => name);
            mockBandDirectory.Setup(directory => directory.Path).Returns(CutPath(bandDirectoryInfo));
            mockBandDirectory.Setup(directory => directory.ToString()).Returns(() => name);
            mockBandDirectory.Setup(directory => directory.Content).Returns(
                albumDirectoriesInfo
                    .Select(ScanAlbum)
                    .Cast<IFsNode>()
                    .OrderBy(fsNode => fsNode.Name)
                    .ToImmutableArray()
            );
            return mockBandDirectory.Object;
        }

        private IDirectory MockAlbum(DirectoryInfo albumDirectoryInfo, FileInfo[] tracksInfo)
        {
            var mockAlbumDirectory = new Mock<IDirectory>(MockBehavior.Strict);
            var name = albumDirectoryInfo.Name;
            mockAlbumDirectory.Setup(directory => directory.Name).Returns(() => name);
            mockAlbumDirectory.Setup(directory => directory.Path).Returns(CutPath(albumDirectoryInfo));
            mockAlbumDirectory.Setup(directory => directory.ToString()).Returns(() => name);
            mockAlbumDirectory.Setup(directory => directory.Rename(It.IsAny<string>()))
                .Callback<string>(newName => name = newName);
            mockAlbumDirectory.Setup(directory => directory.Content).Returns(
                tracksInfo
                    .Select(ScanTrack)
                    .Cast<IFsNode>()
                    .OrderBy(fsNode => fsNode.Name)
                    .ToImmutableArray()
            );
            return mockAlbumDirectory.Object;
        }

        private IFile MockTrack(FileInfo fileInfo)
        {
            var mockTrack = new Mock<IFile>(MockBehavior.Strict);
            var name = fileInfo.Name;
            mockTrack.Setup(file => file.Name).Returns(() => name);
            mockTrack.Setup(file => file.Path).Returns(CutPath(fileInfo));
            mockTrack.Setup(file => file.Extension).Returns(fileInfo.Extension);
            mockTrack.Setup(file => file.ToString()).Returns(() => name);
            mockTrack.Setup(file => file.Rename(It.IsAny<string>()))
                .Callback<string>(newName => name = newName);
            return mockTrack.Object;
        }

        public IDirectoryElement CreateDirectoryElement(string path,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler)
        {
            return new DirectoryElement(this, _fsNodes[path] as IDirectory, checkStateChangeHandler);
        }

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler)
        {
            var path = directory.Path;
            return new DirectoryElement(this, _fsNodes[path] as IDirectory, checkStateChangeHandler);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler)
        {
            var path = file.Path;
            return new FileElement(_fsNodes[path] as IFile, checkStateChangeHandler);
        }
    }
}
