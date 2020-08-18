using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Core.FileSystem;
using LanguageExt;
using Moq;
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
        }

        private readonly TestFilesLoader _testFilesLoader;

        private readonly Dictionary<string, IFsNode<object>> _fsNodes = new Dictionary<string, IFsNode<object>>();

        private readonly string _fileSystemRoot;

        private string CompilationPath { get; set; }

        private string BandPath { get; set; }

        private string AlbumPath { get; set; }

        public IDirectoryElement CompilationDirectoryElement =>
            CreateDirectoryElement(CompilationPath).IfLeft(e => throw e);

        public IDirectoryElement BandDirectoryElement => CreateDirectoryElement(BandPath).IfLeft(e => throw e);

        public IDirectoryElement AlbumDirectoryElement => CreateDirectoryElement(AlbumPath).IfLeft(e => throw e);

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
            mockCompilationDirectory.Setup(d => d.Name).Returns(() => name);
            mockCompilationDirectory.Setup(d => d.Path).Returns(CompilationPath);
            mockCompilationDirectory.Setup(d => d.ToString()).Returns(() => name);
            mockCompilationDirectory.Setup(d => d.Content).Returns(
                bandDirectoriesInfo
                    .Select(ScanBand)
                    .Cast<IFsNode<object>>()
                    .OrderBy(fsNode => fsNode.Name)
                    .ToImmutableArray()
            );
            return mockCompilationDirectory.Object;
        }

        private IDirectory MockBand(DirectoryInfo bandDirectoryInfo,
            Dictionary<DirectoryInfo, FileInfo[]> albumDirectoriesInfo)
        {
            var mockBandDirectory = new Mock<IDirectory>(MockBehavior.Strict);
            var name = mockBandDirectory.Name;
            mockBandDirectory.Setup(d => d.Name).Returns(() => name);
            mockBandDirectory.Setup(d => d.Path).Returns(CutPath(bandDirectoryInfo));
            mockBandDirectory.Setup(d => d.ToString()).Returns(() => name);
            mockBandDirectory.Setup(d => d.Content).Returns(
                albumDirectoriesInfo
                    .Select(ScanAlbum)
                    .Cast<IFsNode<object>>()
                    .OrderBy(fsNode => fsNode.Name)
                    .ToImmutableArray()
            );
            return mockBandDirectory.Object;
        }

        private IDirectory MockAlbum(DirectoryInfo albumDirectoryInfo, FileInfo[] tracksInfo)
        {
            var mockAlbumDirectory = new Mock<IDirectory>(MockBehavior.Strict);
            var name = albumDirectoryInfo.Name;
            mockAlbumDirectory.Setup(d => d.Name).Returns(() => name);
            mockAlbumDirectory.Setup(d => d.Path).Returns(CutPath(albumDirectoryInfo));
            mockAlbumDirectory.Setup(d => d.ToString()).Returns(() => name);
            mockAlbumDirectory.Setup(d => d.Rename(It.IsAny<string>()))
                .Callback<string>(newName => name = newName);
            mockAlbumDirectory.Setup(d => d.Content).Returns(
                tracksInfo
                    .Select(ScanTrack)
                    .Cast<IFsNode<object>>()
                    .OrderBy(fsNode => fsNode.Name)
                    .ToImmutableArray()
            );
            return mockAlbumDirectory.Object;
        }

        private IFile MockTrack(FileInfo fileInfo)
        {
            var mockTrack = new Mock<IFile>(MockBehavior.Strict);
            var name = fileInfo.Name;
            mockTrack.Setup(f => f.Name).Returns(() => name);
            mockTrack.Setup(f => f.Path).Returns(CutPath(fileInfo));
            mockTrack.Setup(f => f.Extension).Returns(fileInfo.Extension);
            mockTrack.Setup(f => f.ToString()).Returns(() => name);
            mockTrack.Setup(f => f.Rename(It.IsAny<string>()))
                .Callback<string>(newName => name = newName);
            return mockTrack.Object;
        }

        public Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path)
        {
            return new DirectoryElement(this, _fsNodes[path] as IDirectory);
        }

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory)
        {
            var path = directory.Path;
            return new DirectoryElement(this, _fsNodes[path] as IDirectory);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file)
        {
            var path = file.Path;
            return new FileElement(_fsNodes[path] as IFile);
        }
    }
}
