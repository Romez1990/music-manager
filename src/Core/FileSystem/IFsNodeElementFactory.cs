using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystem
{
    public interface IFsNodeElementFactory
    {
        Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path);
    }
}
