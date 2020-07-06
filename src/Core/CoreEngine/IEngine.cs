using System.IO;
using LanguageExt;

namespace Core.CoreEngine
{
    public interface IEngine
    {
        Either<DirectoryNotFoundException, IEngineScanner> SetDirectory(string path);
    }
}
