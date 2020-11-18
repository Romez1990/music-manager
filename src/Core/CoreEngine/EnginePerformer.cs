using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.FileSystem;
using Core.Operations.Operation;

namespace Core.CoreEngine
{
    public class EnginePerformer : IEnginePerformer
    {
        internal EnginePerformer(IOperationRepository operationRepository, IDirectoryElement directory,
            Mode mode)
        {
            _operationRepository = operationRepository;
            DirectoryElement = directory;
            _mode = mode;
        }

        private readonly IOperationRepository _operationRepository;

        public IDirectoryElement DirectoryElement { get; }

        private readonly Mode _mode;

        public IDirectoryElement PerformOperations(ImmutableArray<string> operations) =>
            PerformOperations(_operationRepository.FindAllByNames(operations));

        private IDirectoryElement PerformOperations(IEnumerable<IOperation> operations) =>
            operations.Aggregate(DirectoryElement,
                (directory, operation) => operation.Perform(directory, _mode));
    }
}
