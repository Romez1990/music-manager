using System.Collections.Generic;
using System.Collections.Immutable;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operations.Operation;
using Moq;
using NUnit.Framework;

namespace Core.Test.CoreEngine
{
    [TestFixture]
    public class EnginePerformerTest
    {
        [SetUp]
        public void SetUp()
        {
            _operationRepository = new Mock<IOperationRepository>(MockBehavior.Strict);
            _directoryElement = new Mock<IDirectoryElement>().Object;
            _enginePerformer = new EnginePerformer(_operationRepository.Object, _directoryElement, Mode.Band);
        }

        private Mock<IOperationRepository> _operationRepository;

        private IDirectoryElement _directoryElement;

        private IEnginePerformer _enginePerformer;

        [Test]
        public void Engine_PerformsOperationsWhere_Correctly()
        {
            var operation1 = new Mock<IOperation>();
            var operation2 = new Mock<IOperation>();
            _operationRepository.Setup(r => r.FindAllByNames(It.IsAny<IEnumerable<string>>())).Returns(
                new List<IOperation>
                {
                    operation1.Object,
                    operation2.Object,
                });

            _enginePerformer.PerformOperations(new ImmutableArray<string>());

            _operationRepository.Verify(r => r.FindAllByNames(It.IsAny<IEnumerable<string>>()), Times.Once());
            operation1.Verify(o => o.Perform(_directoryElement, Mode.Band), Times.Once());
            operation2.Verify(o => o.Perform(_directoryElement, Mode.Band), Times.Once());
        }
    }
}
