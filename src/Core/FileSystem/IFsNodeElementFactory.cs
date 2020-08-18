using System.IO;
using LanguageExt;

namespace Core.FileSystem
{
    public interface IFsNodeElementFactory
    {
        Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path);
    }
}
