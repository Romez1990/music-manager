using Core.CoreEngine;
using Core.FileSystem;
using Core.Operation;
using Moq;
using NUnit.Framework;
using Utils.Reflection;

namespace Core.Test.CoreEngine
{
    [TestFixture]
    public class EnginePerformerTest
    {
        [SetUp]
        public void SetUp()
        {
            _directoryElement = new Mock<IDirectoryElement>().Object;
            _enginePerformer = ReflectionHelper.Construct<EnginePerformer>(_directoryElement, Mode.Band);
        }

        private IDirectoryElement _directoryElement;

        private IEnginePerformer _enginePerformer;

        [Test]
        public void Engine_PerformsActions_Correctly()
        {
            var operation1 = new Mock<IOperation>();
            var operation2 = new Mock<IOperation>();

            _enginePerformer.PerformOperations(new[] {operation1.Object, operation2.Object});

            operation1.Verify(o => o.Perform(_directoryElement, Mode.Band), Times.Once());
            operation2.Verify(o => o.Perform(_directoryElement, Mode.Band), Times.Once());
        }
    }
}
