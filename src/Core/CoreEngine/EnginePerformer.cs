using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.FileSystem;
using Core.Operations.Operation;

namespace Core.CoreEngine
{
    public class EnginePerformer : IEnginePerformer
    {
        internal EnginePerformer(IOperationRepository operationRepository, IDirectoryElement directoryElement,
            Mode mode)
        {
            _operationRepository = operationRepository;
            DirectoryElement = directoryElement;
            _mode = mode;
        }

        private readonly IOperationRepository _operationRepository;

        public IDirectoryElement DirectoryElement { get; }

        private readonly Mode _mode;

        public IDirectoryElement PerformOperations(ImmutableArray<string> operations)
        {
            return PerformOperations(_operationRepository.FindAllByNames(operations));
        }

        private IDirectoryElement PerformOperations(IEnumerable<IOperation> operations)
        {
            return operations.Aggregate(DirectoryElement,
                (directoryElement, operation) => operation.Perform(directoryElement, _mode));
        }
    }
}
