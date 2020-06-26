using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;
using Core.FileSystem;

namespace Core.Test.FileSystem
{
    public class MockFsInfoFactory : IFsInfoFactory
    {
        public MockFsInfoFactory()
        {
            FsRoot = Directory.GetDirectoryRoot(Assembly.GetExecutingAssembly().Location);
            _mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {Path.Combine(DirectoryPath, "sub_directory", "file3.ext"), new MockFileData("")},
                {FilePath, new MockFileData("")},
                {Path.Combine(DirectoryPath, "file2.ext"), new MockFileData("")},
            });
        }

        private readonly IMockFileDataAccessor _mockFileSystem;

        public readonly string FsRoot;

        public string DirectoryPath => Path.Combine(FsRoot, "directory");

        public string FilePath => Path.Combine(DirectoryPath, "file1.ext");

        public IDirectoryInfo DirectoryInfo => CreateDirectoryInfo(DirectoryPath);

        public IFileInfo FileInfo => CreateFileInfo(FilePath);

        public IDirectoryInfo CreateDirectoryInfo(string path)
        {
            return new MockDirectoryInfo(_mockFileSystem, path);
        }

        public IFileInfo CreateFileInfo(string path)
        {
            return new MockFileInfo(_mockFileSystem, path);
        }
    }
}
