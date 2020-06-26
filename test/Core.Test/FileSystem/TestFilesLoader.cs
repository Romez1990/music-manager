using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Test.FileSystem
{
    public class TestFilesLoader
    {
        public TestFilesLoader()
        {
            var solutionDirectory = _filesHelper.GetSolutionDirectory();
            TestFilesDirectoryPath = Path.Combine(solutionDirectory.FullName, "test", "test_compilation");
            _testFilesDirectoryInfo = new DirectoryInfo(TestFilesDirectoryPath);
        }

        private readonly FilesHelper _filesHelper = new FilesHelper();

        public string TestFilesDirectoryPath { get; }

        private readonly DirectoryInfo _testFilesDirectoryInfo;

        public Dictionary<DirectoryInfo, Dictionary<DirectoryInfo, FileInfo[]>> GetFiles()
        {
            return _testFilesDirectoryInfo
                .GetDirectories()
                .ToDictionary(
                    bandDirectoryInfo => bandDirectoryInfo,
                    bandDirectoryInfo => bandDirectoryInfo
                        .GetDirectories()
                        .ToDictionary(
                            albumDirectoryInfo => albumDirectoryInfo,
                            albumDirectoryInfo => albumDirectoryInfo
                                .GetFiles()
                        )
                );
        }
    }
}
