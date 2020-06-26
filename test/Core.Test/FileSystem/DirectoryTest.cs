using Core.FileSystem;
using NUnit.Framework;
using Path = System.IO.Path;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class DirectoryTest
    {
        [SetUp]
        public void SetUp()
        {
            _fsInfoFactory = new MockFsInfoFactory();
            _directory = new Directory(_fsInfoFactory.DirectoryInfo);
        }

        private MockFsInfoFactory _fsInfoFactory;

        private IDirectory _directory;

        [Test]
        public void Directory_Renames_Correctly()
        {
            const string newName = "directory2";
            var newPath = Path.Combine(_fsInfoFactory.FsRoot, newName);

            _directory.Rename(newName);

            Assert.That(_fsInfoFactory.CreateDirectoryInfo(newPath).Exists, Is.True);
            Assert.That(_fsInfoFactory.CreateDirectoryInfo(_fsInfoFactory.DirectoryPath).Exists, Is.False);
        }
    }
}
