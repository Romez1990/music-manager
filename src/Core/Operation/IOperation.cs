using Core.FileSystemElement;
using Core.OperationEngine;

namespace Core.Operation {
    public interface IOperation {
        string Name { get; }
        string Description { get; }
        OperationResult Perform(IDirectoryElement directory, DirectoryMode directoryMode);
    }
}
