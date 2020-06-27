using System.Linq;
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
        public void DirectoryElement_Checks_Correctly()
        {
            _directoryElement.Check();

            Assert.That(_directoryElement.Content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Checked));
        }

        [Test]
        public void DirectoryElement_ChecksPartially_Correctly()
        {
            _directoryElement.Check();

            _directoryElement.Content.First().Uncheck();

            Assert.That(_directoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }

        [Test]
        public void DirectoryElement_ToString_Correctly()
        {
            Assert.That(_directoryElement.ToString(), Is.EqualTo(_directoryElement.FsNode.Name));
        }
    }
}
