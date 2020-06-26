using Core.FileSystem;
using NUnit.Framework;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class DirectoryElementTest
    {
        [SetUp]
        public void SetUp()
        {
            var fsNodeElementFactory = new MockFsNodeElementFactory();
            _directoryElement = fsNodeElementFactory.BandDirectoryElement;
        }

        private IDirectoryElement _directoryElement;

        [Test]
        public void DirectoryElement_Creates_Uncheked()
        {
            Assert.That(_directoryElement.CheckState, Is.EqualTo(CheckState.Unchecked));
        }

        [Test]
        public void DirectoryElement_Checks_Correctly()
        {
            var content = _directoryElement.Content;
            
            _directoryElement.Check();

            Assert.That(content[0].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(content[1].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(content[2].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(content[3].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(content[4].CheckState, Is.EqualTo(CheckState.Checked));
        }

        [Test]
        public void DirectoryElement_ChecksPartially_Correctly()
        {
            var content = _directoryElement.Content;
            _directoryElement.Check();

            content[0].Uncheck();

            Assert.That(_directoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }

        [Test]
        public void DirectoryElement_ToString_Correctly()
        {
            Assert.That(_directoryElement.ToString(), Is.EqualTo(_directoryElement.FsNode.Name));
        }
    }
}
