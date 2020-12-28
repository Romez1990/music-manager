using Core.CoreEngine;
using Core.FileScanner;
using Core.FileSystem;
using Moq;
using NUnit.Framework;

namespace Core.Test.CoreEngine
{
    [TestFixture]
    public class EngineScannerTest
    {
        [SetUp]
        public void SetUp()
        {
            _engineFactory = new Mock<IEngineFactory>(MockBehavior.Strict);
            _scanner = new Mock<IScanner>();
            _directoryElement = new Mock<IDirectoryElement>().Object;
            _engineScanner = new EngineScanner(_engineFactory.Object, _scanner.Object, _directoryElement);
        }

        private IEngineScanner _engineScanner;
        private Mock<IEngineFactory> _engineFactory;
        private Mock<IScanner> _scanner;
        private IDirectoryElement _directoryElement;

        [Test]
        public void ScanAlbum_CallsScanner()
        {
            var enginePerformer = new Mock<IEnginePerformer>().Object;
            _engineFactory.Setup(f => f.CreateEnginePerformer(_directoryElement, Mode.Band)).Returns(enginePerformer);
            var enginePerformerResult = _engineScanner.Scan(Mode.Band);

            _scanner.Verify(s => s.Scan(_directoryElement, Mode.Band), Times.Once());
            Assert.That(enginePerformer, Is.EqualTo(enginePerformerResult));
        }
    }
}
