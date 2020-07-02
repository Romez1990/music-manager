using Core.FileSystem;
using NUnit.Framework;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class DirectoryTest
    {
        [SetUp]
        public void SetUp()
        {
            var fsInfoFactory = new MockFsInfoFactory();
            var fsNodeFactory = new FsNodeFactory(fsInfoFactory);
            _directory = new Directory(fsNodeFactory, fsInfoFactory.DirectoryInfo);
        }

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
