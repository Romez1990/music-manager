using Moq;
using NUnit.Framework;
using Core.CoreEngine;
using Core.FileSystem;
using Core.FileSystem.Exceptions;
using LanguageExt.UnitTesting;
using static LanguageExt.Prelude;

namespace Core.Test.CoreEngine
{
    [TestFixture]
    public class EngineTest
    {
        [SetUp]
        public void SetUp()
        {
            _engineFactory = new Mock<IEngineFactory>(MockBehavior.Strict);
            _fsNodeElementFactory = new Mock<IFsNodeElementFactory>(MockBehavior.Strict);
            _engine = new Engine(_engineFactory.Object, _fsNodeElementFactory.Object);
        }

        private IEngine _engine;
        private Mock<IEngineFactory> _engineFactory;
        private Mock<IFsNodeElementFactory> _fsNodeElementFactory;

        [Test]
        public void SetDirectory_WhenDirectoryExists_ReturnsRight()
        {
            var directoryElement = new Mock<IDirectoryElement>().Object;
            var engineScanner = new Mock<IEngineScanner>().Object;
            _engineFactory.Setup(f => f.CreateEngineScanner(directoryElement)).Returns(engineScanner);
            _fsNodeElementFactory.Setup(f => f.CreateDirectoryElement("123"))
                .Returns(Right(directoryElement));

            var result = _engine.SetDirectory("123");

            result.ShouldBeRight(engineScannerResult =>
            {
                _engineFactory.Verify(f => f.CreateEngineScanner(directoryElement), Times.Once());
                Assert.That(engineScannerResult, Is.EqualTo(engineScanner));
            });
        }

        [Test]
        public void SetDirectory_WhenDirectoryNotExists_ReturnsLeft()
        {
            _fsNodeElementFactory.Setup(f => f.CreateDirectoryElement(It.IsAny<string>()))
                .Returns(Left(new DirectoryNotFoundException("some/path")));

            var result = _engine.SetDirectory("123");

            result.ShouldBeLeft();
        }
    }
}
