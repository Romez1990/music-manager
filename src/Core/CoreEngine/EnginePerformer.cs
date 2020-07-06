using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core.FileSystem;
using Core.Operation;

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

        public void PerformAllOperations()
        {
            PerformOperations(_operationRepository.FindAll());
        }

        public void PerformOperations(ImmutableArray<string> operations)
        {
            PerformOperations(_operationRepository.FindAllByName(operations));
        }

        private void PerformOperations(IEnumerable<IOperation> operations)
        {
            operations
                .ToList()
                .ForEach(operation => operation.Perform(DirectoryElement, _mode));
        }
    }
}
