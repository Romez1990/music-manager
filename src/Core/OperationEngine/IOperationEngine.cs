using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.OperationEngine {
    public interface IOperationEngine {
        Either<DirectoryNotFoundException, IEngineScanner> SetDirectory(string path);
    }
}
