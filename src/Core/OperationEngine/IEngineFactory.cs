using Core.FileSystemElement;

namespace Core.OperationEngine {
    public interface IEngineFactory {
        IEngineScanner CreateScanner(IDirectoryElement directory);
        IEnginePerformer CreateEnginePerformer(IDirectoryElement directory, DirectoryMode directoryMode);
    }
}
