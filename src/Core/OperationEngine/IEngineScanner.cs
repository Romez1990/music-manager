namespace Core.OperationEngine {
    public interface IEngineScanner {
        IEnginePerformer Scan(DirectoryMode directoryMode);
    }
}
