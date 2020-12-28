using System;
using System.Linq;
using Core.FileSystem;
using Moq;
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

        [TestCase(CheckState.Checked, CheckState.Checked, CheckState.Checked, CheckState.Checked, CheckState.Checked)]
        [TestCase(CheckState.Unchecked, CheckState.Unchecked, CheckState.Unchecked, CheckState.Unchecked,
            CheckState.Unchecked)]
        [TestCase(CheckState.Checked, CheckState.Unchecked, CheckState.Checked, CheckState.Unchecked,
            CheckState.CheckedPartially)]
        public void DirectoryElement_SelectContent_Correctly(CheckState fileElement1CheckState,
            CheckState fileElement2CheckState, CheckState fileElement3CheckState, CheckState fileElement4CheckState,
            CheckState resultCheckState)
        {
            var fileElement1 = new Mock<IFileElement>(MockBehavior.Strict);
            fileElement1.Setup(f => f.CheckState).Returns(fileElement1CheckState);
            var fileElement2 = new Mock<IFileElement>(MockBehavior.Strict);
            fileElement2.Setup(f => f.CheckState).Returns(fileElement2CheckState);
            var fileElement3 = new Mock<IFileElement>(MockBehavior.Strict);
            fileElement3.Setup(f => f.CheckState).Returns(fileElement3CheckState);
            var fileElement4 = new Mock<IFileElement>(MockBehavior.Strict);
            fileElement4.Setup(f => f.CheckState).Returns(fileElement4CheckState);
            var function = new Mock<Func<IFsNodeElement<object>, IFsNodeElement<object>>>(MockBehavior.Strict);
            function.SetupSequence(f => f(It.IsAny<IFsNodeElement<object>>()))
                .Returns(fileElement1.Object)
                .Returns(fileElement2.Object)
                .Returns(fileElement3.Object)
                .Returns(fileElement4.Object);

            var newDirectoryElement = _directoryElement.MapContent(function.Object);

            Assert.That(newDirectoryElement.CheckState, Is.EqualTo(resultCheckState));
            Assert.That(newDirectoryElement.Content[0], Is.EqualTo(fileElement1.Object));
            Assert.That(newDirectoryElement.Content[1], Is.EqualTo(fileElement2.Object));
            Assert.That(newDirectoryElement.Content[2], Is.EqualTo(fileElement3.Object));
            Assert.That(newDirectoryElement.Content[3], Is.EqualTo(fileElement4.Object));
        }

        [Test]
        public void DirectoryElement_ToString_Correctly()
        {
            Assert.That(_directoryElement.ToString(), Is.EqualTo(_directoryElement.Name));
        }
    }
}
