using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Core.FileSystem;

namespace Core.Test.FileSystem
{
    public class MockFileSystemInfoFactory : IFileSystemInfoFactory
    {
        public MockFileSystemInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            _mockFileSystem = mockFileSystem;
        }

        private readonly IMockFileDataAccessor _mockFileSystem;

        public IFileInfo CreateFileInfo(string path)
        {
            return new MockFileInfo(_mockFileSystem, path);
        }

        public IDirectoryInfo CreateDirectoryInfo(string path)
        {
            return new MockDirectoryInfo(_mockFileSystem, path);
        }
    }
}
