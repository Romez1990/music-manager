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
            var fsNodeFactory = new FsNodeFactory(_fsInfoFactory);
            _directory = new Directory(fsNodeFactory, _fsInfoFactory, _fsInfoFactory.DirectoryInfo);
        }

        private MockFsInfoFactory _fsInfoFactory;

        private IDirectory _directory;

        [Test]
        public void Directory_Content()
        {
            var content = _directory.Content;

            Assert.That(content.Length, Is.EqualTo(3));
            Assert.That(content[0].Name, Is.EqualTo("sub_directory"));
            Assert.That(content[1].Name, Is.EqualTo("file1.ext"));
            Assert.That(content[2].Name, Is.EqualTo("file2.ext"));
        }
    }
}
