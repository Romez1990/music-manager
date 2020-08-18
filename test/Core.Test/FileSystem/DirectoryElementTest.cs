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
            var newDirectoryElement = _directoryElement.Check();

            Assert.That(newDirectoryElement.CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(newDirectoryElement.Content.All(fsNodeElement =>
                fsNodeElement.CheckState == CheckState.Checked));
        }

        [Test]
        public void DirectoryElement_Unchecks_Correctly()
        {
            var newDirectoryElement = _directoryElement.Uncheck();

            Assert.That(newDirectoryElement.CheckState, Is.EqualTo(CheckState.Unchecked));
            Assert.That(newDirectoryElement.Content.All(fsNodeElement =>
                fsNodeElement.CheckState == CheckState.Unchecked));
        }

        [Test]
        public void DirectoryElement_ToString_Correctly()
        {
            Assert.That(_directoryElement.ToString(), Is.EqualTo(_directoryElement.Name));
        }
    }
}
