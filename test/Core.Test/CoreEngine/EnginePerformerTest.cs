using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operations;
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
            _enginePerformer = new EnginePerformer(_operationRepository.Object, _directoryElement, PerformingMode);
        }

        private EnginePerformer _enginePerformer;
        private Mock<IOperationRepository> _operationRepository;
        private IDirectoryElement _directoryElement;
        private const Mode PerformingMode = Mode.Band;

        [Test]
        public void PerformOperations_CallsOperations()
        {
            var operation1 = new Mock<IOperation>();
            var operation2 = new Mock<IOperation>();
            var directory1 = new Mock<IDirectoryElement>().Object;
            var directory2 = new Mock<IDirectoryElement>().Object;
            operation1.Setup(o => o.Perform(It.IsAny<IDirectoryElement>(), It.IsAny<Mode>()))
                .Returns(new OperationResult(directory1, Enumerable.Empty<OperationException>()));
            operation2.Setup(o => o.Perform(It.IsAny<IDirectoryElement>(), It.IsAny<Mode>()))
                .Returns(new OperationResult(directory2, Enumerable.Empty<OperationException>()));
            var operationNames = new ImmutableArray<string>();
            _operationRepository.Setup(r => r.FindAllByNames(operationNames)).Returns(
                new List<IOperation>
                {
                    operation1.Object,
                    operation2.Object,
                });

            var result = _enginePerformer.PerformOperations(operationNames);

            _operationRepository.Verify(r => r.FindAllByNames(operationNames), Times.Once());
            operation1.Verify(o => o.Perform(_directoryElement, PerformingMode), Times.Once());
            operation2.Verify(o => o.Perform(directory1, PerformingMode), Times.Once());
            Assert.That(result.Directory, Is.EqualTo(directory2));
            Assert.That(result.Exceptions.Any(), Is.False);
        }
    }
}
