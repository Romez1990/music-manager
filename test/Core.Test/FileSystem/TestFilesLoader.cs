using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Test.Utils;

namespace Core.Test.FileSystem
{
    public class TestFilesLoader
    {
        public TestFilesLoader()
        {
            var filesHelper = new FsHelper();
            var solutionDirectory = filesHelper.GetSolutionDirectory();
            TestFilesDirectoryPath = Path.Combine(solutionDirectory.FullName, "test", "TestCompilation");
            _testFilesDirectoryInfo = new DirectoryInfo(TestFilesDirectoryPath);
        }

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
