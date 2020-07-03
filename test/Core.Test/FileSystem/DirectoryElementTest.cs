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
            _fsNodeElementFactory = new MockFsNodeElementFactory();
        }

        private MockFsNodeElementFactory _fsNodeElementFactory;

        private IDirectoryElement DirectoryElement => _fsNodeElementFactory.BandDirectoryElement;

        [Test]
        public void DirectoryElement_Checks_Correctly()
        {
            DirectoryElement.Check();

            Assert.That(DirectoryElement.Content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Checked));
        }

        [Test]
        public void DirectoryElement_ChecksPartially_Correctly()
        {
            DirectoryElement.Check();

            DirectoryElement.Content.First().Uncheck();

            Assert.That(DirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }

        [Test]
        public void DirectoryElement_ToString_Correctly()
        {
            Assert.That(DirectoryElement.ToString(), Is.EqualTo(DirectoryElement.FsNode.Name));
        }
    }
}
