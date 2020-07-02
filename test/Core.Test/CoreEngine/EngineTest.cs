using System;
using System.Reflection;
using Core.CoreEngine;
using Core.FileScanner;
using Core.FileSystem;
using Core.Operation;
using Moq;
using NUnit.Framework;

namespace Core.Test.CoreEngine
{
    [TestFixture]
    public class EngineTest
    {
        [SetUp]
        public void SetUp()
        {
            _fsNodeElementFactory = new Mock<IFsNodeElementFactory>();
            _scanner = new Mock<IScanner>();
            _engine = new Engine(
                _fsNodeElementFactory.Object,
                _scanner.Object
            );
        }

        private Mock<IFsNodeElementFactory> _fsNodeElementFactory;

        private Mock<IScanner> _scanner;

        private IEngine _engine;

        [Test]
        public void Engine_SetsDirectory_Correctly()
        {
            var directoryElement = new Mock<IDirectoryElement>();
            directoryElement.Setup(d => d.FsNode.Exists).Returns(true);
            _fsNodeElementFactory.Setup(f =>
                    f.CreateDirectoryElement(It.IsAny<string>(), It.IsAny<EventHandler<FsNodeElementCheckEventArgs>>()))
                .Returns(directoryElement.Object);

            var result = _engine.SetDirectory("");

            Assert.That(result, Is.True);
        }

        [Test]
        public void Engine_SetDirectoryFails_WhenDirectoryDoesNotExist()
        {
            var directoryElement = new Mock<IDirectoryElement>();
            directoryElement.Setup(d => d.FsNode.Exists).Returns(false);
            _fsNodeElementFactory.Setup(f =>
                    f.CreateDirectoryElement(It.IsAny<string>(), It.IsAny<EventHandler<FsNodeElementCheckEventArgs>>()))
                .Returns(directoryElement.Object);

            var result = _engine.SetDirectory("");

            Assert.That(result, Is.False);
        }

        [Test]
        public void Engine_ScansAlbum_Correctly()
        {
            var directoryElement = new Mock<IDirectoryElement>();
            directoryElement.Setup(d => d.FsNode.Exists).Returns(true);
            _fsNodeElementFactory.Setup(f =>
                    f.CreateDirectoryElement(It.IsAny<string>(), It.IsAny<EventHandler<FsNodeElementCheckEventArgs>>()))
                .Returns(directoryElement.Object);
            _engine.SetDirectory("");

            _engine.Scan(Mode.Album);

            _scanner.Verify(s => s.Scan(directoryElement.Object, Mode.Album), Times.Once());
        }

        [Test]
        public void Engine_ScanAlbumFails_WhenScanBeforeSetDirectory()
        {
            void Scan() => _engine.Scan(Mode.Album);

            Assert.That(Scan,
                Throws.Exception.TypeOf<ApplicationException>().With.Property("Message")
                    .EqualTo("Cannot scan before set directory"));
            _scanner.Verify(s => s.Scan(It.IsAny<IDirectoryElement>(), It.IsAny<Mode>()), Times.Never());
        }

        [Test]
        public void Engine_PerformsActions_Correctly()
        {
            var directoryElement = new Mock<IDirectoryElement>();
            directoryElement.Setup(d => d.CheckState).Returns(CheckState.Checked);
            // ReSharper disable once PossibleNullReferenceException
            _engine.GetType().GetProperty("DirectoryElement")
                .SetValue(_engine, directoryElement.Object);
            // ReSharper disable once PossibleNullReferenceException
            _engine.GetType().GetField("_mode", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(_engine, Mode.Band);
            var operation1 = new Mock<IOperation>();
            var operation2 = new Mock<IOperation>();

            _engine.PerformActions(new[] {operation1.Object, operation2.Object});

            operation1.Verify(o => o.Perform(directoryElement.Object, Mode.Band), Times.Once());
            operation2.Verify(o => o.Perform(directoryElement.Object, Mode.Band), Times.Once());
        }

        [Test]
        public void Engine_PerformActionsFails_WhenNothingIsChecked()
        {
            var directoryElement = new Mock<IDirectoryElement>();
            typeof(Engine).GetProperty("DirectoryElement")?.SetValue(_engine, directoryElement.Object);
            var operation1 = new Mock<IOperation>();
            var operation2 = new Mock<IOperation>();

            void Perform() => _engine.PerformActions(new[] {operation1.Object, operation2.Object});

            Assert.That(Perform,
                Throws.Exception.TypeOf<ApplicationException>().With.Property("Message")
                    .EqualTo("Directory is unchecked"));
            operation1.Verify(o => o.Perform(It.IsAny<IDirectoryElement>(), It.IsAny<Mode>()), Times.Never());
            operation2.Verify(o => o.Perform(It.IsAny<IDirectoryElement>(), It.IsAny<Mode>()), Times.Never());
        }
    }
}
