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

        public OperationResult PerformOperations(ImmutableArray<string> operations) =>
            PerformOperations(_operationRepository.FindAllByNames(operations));

        private OperationResult PerformOperations(IEnumerable<IOperation> operations) =>
            operations.Aggregate(new OperationResult(DirectoryElement, Enumerable.Empty<OperationException>()),
                (resultAccumulator, operation) =>
                {
                    var result = operation.Perform(resultAccumulator.Directory, _mode);
                    return new OperationResult(result.Directory,
                        resultAccumulator.Exceptions.Concat(result.Exceptions));
                });
    }
}
