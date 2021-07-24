using System.Collections.Generic;
using System.Collections.Immutable;
using Core.FileSystemElement;
using Core.Operation;

namespace Core.OperationEngine {
    public class EnginePerformer : IEnginePerformer {
        public EnginePerformer(IOperationRepository operations, IDirectoryElement directory,
            DirectoryMode directoryMode) {
            _operations = operations;
            _directoryMode = directoryMode;
            Directory = directory;
        }

        private readonly IOperationRepository _operations;
        private readonly DirectoryMode _directoryMode;
        public IDirectoryElement Directory { get; }

        public OperationResult PerformOperations(IEnumerable<string> operations) =>
            PerformOperations(_operations.FindByNames(operations));

        public OperationResult PerformOperations(IEnumerable<IOperation> operations) =>
            operations.Fold(
                new OperationResult(Directory, ImmutableList<OperationException>.Empty),
                (resultAccumulator, operation) =>
                    resultAccumulator + operation.Perform(resultAccumulator.Directory, _directoryMode)
            );
    }
}
