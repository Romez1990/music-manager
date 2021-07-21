using Core.FileSystem;
using Core.FileSystemElement;
using Core.FileSystemElement.Exceptions;
using LanguageExt.UnitTesting;
using Moq;
using NUnit.Framework;
using Utils.Reflection;
using static LanguageExt.Prelude;

namespace Core.Test.FileSystemElement {
    [TestFixture]
    public class FileElementTest {
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            _fsNodeElementFactory = new FsNodeElementFactory(null);
        }

        [SetUp]
        public void SetUp() {
            _file = new Mock<IFile>(MockBehavior.Strict);
            _fileElement = new FileElement(_fsNodeElementFactory, _file.Object, CheckState.Unchecked);
            _fileElement.Changed += (_, e) => _changedFileElement = (FileElement)e.FsNodeElement;
            _changedFileElement = null;
        }

        private FileElement _fileElement;
        private FileElement _changedFileElement;
        private FsNodeElementFactory _fsNodeElementFactory;
        private Mock<IFile> _file;

        [Test]
        public void Rename() {
            const string newName = "new name.txt";
            var newFile = new Mock<IFile>();
            _file.Setup(f => f.Rename(newName)).Returns(Right(newFile.Object));

            var newFileElementEither = _fileElement.Rename(newName);

            _file.Verify(f => f.Rename(newName), Times.Once);
            newFileElementEither.ShouldBeRight(newFileElement => {
                Assert.That(ReferenceEquals(_changedFileElement, newFileElement), Is.True);
                Assert.That(ReferenceEquals(newFileElement.GetPropertyValue<IFsNode>("FsNode"), newFile.Object),
                    Is.True);
            });
        }

        [Test]
        public void Check_ReturnsCheckedFile() {
            var newFileElement = _fileElement.Check();

            Assert.That(ReferenceEquals(_changedFileElement, newFileElement), Is.True);
            Assert.That(newFileElement.CheckState, Is.EqualTo(CheckState.Checked));
        }

        [Test]
        public void Check_WhenIsAlreadyChecked_ThrowsException() {
            var checkedFileElement = _fileElement.Check();

            Assert.Throws<CheckStateException>(() => checkedFileElement.Check());
        }

        [Test]
        public void Check_WhenIsAlreadyCheckedAndIgnore_ReturnsSameFileElement() {
            var checkedFileElement = _fileElement.Check();

            var newFileElement = checkedFileElement.Check(true);

            Assert.That(ReferenceEquals(newFileElement, checkedFileElement), Is.True);
        }
    }
}
