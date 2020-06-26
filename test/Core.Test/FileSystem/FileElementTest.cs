using Core.FileSystem;
using NUnit.Framework;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class FileElementTest
    {
        [SetUp]
        public void SetUp()
        {
            var fsNodeElementFactory = new MockFsNodeElementFactory();
            _fileElement = fsNodeElementFactory.TrackFileElement;
        }

        private IFileElement _fileElement;

        [Test]
        public void FileElement_Creates_Unchecked()
        {
            Assert.That(_fileElement.CheckState, Is.EqualTo(CheckState.Unchecked));
        }

        [Test]
        public void FileElement_Checks_Correctly()
        {
            _fileElement.Check();

            Assert.That(_fileElement.CheckState, Is.EqualTo(CheckState.Checked));
        }

        [Test]
        public void FileElement_Unchecks_Correctly()
        {
            _fileElement.Check();
            _fileElement.Uncheck();

            Assert.That(_fileElement.CheckState, Is.EqualTo(CheckState.Unchecked));
        }

        [Test]
        public void FileElement_ToString_Correctly()
        {
            Assert.That(_fileElement.ToString(), Is.EqualTo(_fileElement.FsNode.Name));
        }
    }
}
