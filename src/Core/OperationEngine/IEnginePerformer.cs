using System.Collections.Generic;
using Core.FileSystemElement;
using Core.Operation;

namespace Core.OperationEngine {
    public interface IEnginePerformer {
        IDirectoryElement Directory { get; }
        OperationResult PerformOperations(IEnumerable<string> operations);
        OperationResult PerformOperations(IEnumerable<IOperation> operations);
    }
}
