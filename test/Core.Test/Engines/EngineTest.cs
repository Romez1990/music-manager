using Core.Actions;
using Core.Engines;
using Core.FileSystem;
using Core.Scanners;
using Moq;
using NUnit.Framework;

namespace Core.Test.Engines
{
    public class EngineTest
    {
        [Test]
        public void Engine_SetsDirectory_Correctly()
        {
            var mockFileSystemInfoFactory = new Mock<IFileSystemInfoFactory>(MockBehavior.Strict);
            var mockFsNodeElementFactory = new Mock<IFsNodeElementFactory>(MockBehavior.Strict);
            var mockScanner = new Mock<IScanner>(MockBehavior.Strict);
            var mockActionsMapper = new Mock<IActionsMapper>(MockBehavior.Strict);
            var engine = new Engine(
                mockFileSystemInfoFactory.Object,
                mockFsNodeElementFactory.Object,
                mockScanner.Object,
                mockActionsMapper.Object
            );

            //engine.SetDirectory("");

            //Assert.That(engine.DirectoryElement.Content[0].CheckState, Is.EqualTo(CheckState.Checked));
            //Assert.That(engine.DirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }

        [Test]
        public void Engine_ScansAlbum_Correctly()
        {
            var mockFileSystemInfoFactory = new Mock<IFileSystemInfoFactory>(MockBehavior.Strict);
            var mockFsNodeElementFactory = new Mock<IFsNodeElementFactory>(MockBehavior.Strict);
            var mockScanner = new Mock<IScanner>(MockBehavior.Strict);
            var mockActionsMapper = new Mock<IActionsMapper>(MockBehavior.Strict);
            var engine = new Engine(
                mockFileSystemInfoFactory.Object,
                mockFsNodeElementFactory.Object,
                mockScanner.Object,
                mockActionsMapper.Object
            );

            //engine.Scan(Mode.Album);

            //Assert.That(engine.DirectoryElement.Content[0].CheckState, Is.EqualTo(CheckState.Checked));
            //Assert.That(engine.DirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }

        [Test]
        public void Engine_PerformsActions_Correctly()
        {
            var mockFileSystemInfoFactory = new Mock<IFileSystemInfoFactory>(MockBehavior.Strict);
            var mockFsNodeElementFactory = new Mock<IFsNodeElementFactory>(MockBehavior.Strict);
            var mockScanner = new Mock<IScanner>(MockBehavior.Strict);
            var mockActionsMapper = new Mock<IActionsMapper>(MockBehavior.Strict);
            var engine = new Engine(
                mockFileSystemInfoFactory.Object,
                mockFsNodeElementFactory.Object,
                mockScanner.Object,
                mockActionsMapper.Object
            );

            //engine.PerformActions(Action.Rename);
        }
    }
}
