using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.CoreEngine
{
    public interface IEngine
    {
        Either<DirectoryNotFoundException, IEngineScanner> SetDirectory(string path);
    }
}
