using System;
using System.Collections.Generic;
using System.Linq;
using Core.FileSystem;
using Core.FileSystemElement;
using LanguageExt.UnitTesting;
using Moq;
using NUnit.Framework;
using Utils.Reflection;
using static LanguageExt.Prelude;

namespace Core.Test.FileSystemElement {
    [TestFixture]
    public class DirectoryElementTest {
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            _fsNodeElementFactory = new FsNodeElementFactory(null);
        }

        [SetUp]
        public void SetUp() {
            _directory = new Mock<IDirectory>(MockBehavior.Strict) { DefaultValue = DefaultValue.Mock };
            _directoryElement = new DirectoryElement(_fsNodeElementFactory, _directory.Object, CheckState.Unchecked,
                false, ChildrenRetrieval<IFsNodeElement>.Create());
            _directoryElement.RootChanged += (_, e) =>
                _changedDirectoryElement = (DirectoryElement)e.Directory;
            _changedDirectoryElement = null;
            _directoryChildren = new IFsNode[] {
                CreateFile(),
                CreateFile(),
            };
        }

        private IFile CreateFile() {
            var mock = new Mock<IFile>(MockBehavior.Strict);
            var file = mock.Object;
            IFsNodeElement matchResult = null;
            mock.Setup(f =>
                    f.Match(It.IsAny<Func<IDirectory, IFsNodeElement>>(), It.IsAny<Func<IFile, IFsNodeElement>>()))
                .Callback((Func<IDirectory, IFsNodeElement> _, Func<IFile, IFsNodeElement> onFile) =>
                    matchResult = onFile(file))
                .Returns(() => matchResult);
            return file;
        }

        private DirectoryElement _directoryElement;
        private DirectoryElement _changedDirectoryElement;
        private FsNodeElementFactory _fsNodeElementFactory;
        private Mock<IDirectory> _directory;
        private IReadOnlyList<IFsNode> _directoryChildren;

        [Test]
        public void Rename() {
            const string newName = "new name";
            _directory.SetupGet(d => d.Children).Returns(_directoryChildren);
            var newDirectory = new Mock<IDirectory>();
            newDirectory.SetupGet(d => d.Children).Returns(_directoryChildren);
            _directory.Setup(d => d.Rename(newName)).Returns(Right(newDirectory.Object));
            var fileElement1 = (FileElement)_directoryElement.Children[0];
            fileElement1.Check();

            var newDirectoryElementEither = _changedDirectoryElement.Rename(newName);

            _directory.Verify(d => d.Rename(newName), Times.Once);
            newDirectoryElementEither.ShouldBeRight(newDirectoryElement => {
                Assert.That(ReferenceEquals(_changedDirectoryElement, newDirectoryElement), Is.True);
                Assert.That(
                    ReferenceEquals(newDirectoryElement.GetPropertyValue<IFsNode>("FsNode"), newDirectory.Object),
                    Is.True);
            });
            var newFileElement1 = _changedDirectoryElement.Children[0];
            Assert.That(newFileElement1.CheckState, Is.EqualTo(CheckState.Checked));
        }

        [Test]
        public void Check() {
            _directory.SetupGet(d => d.Children).Returns(_directoryChildren);

            var newDirectoryElement = _directoryElement.Check();

            Assert.That(newDirectoryElement.CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(newDirectoryElement.Children.All(n => n.CheckState is CheckState.Checked), Is.True);
        }

        [Test]
        public void ChildrenCheck() {
            _directory.SetupGet(d => d.Children).Returns(_directoryChildren);

            var fileElement1 = (FileElement)_directoryElement.Children[0];
            fileElement1.Check();
            Assert.That(_changedDirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));

            var fileElement2 = (FileElement)_changedDirectoryElement.Children[1];
            fileElement2.Check();
            Assert.That(_changedDirectoryElement.CheckState, Is.EqualTo(CheckState.Checked));

            fileElement2 = (FileElement)_changedDirectoryElement.Children[1];
            fileElement2.Uncheck();
            Assert.That(_changedDirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));

            fileElement1 = (FileElement)_changedDirectoryElement.Children[0];
            fileElement1.Uncheck();
            Assert.That(_changedDirectoryElement.CheckState, Is.EqualTo(CheckState.Unchecked));
        }
    }
}
