using System.IO.Abstractions;
using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystem
{
    public interface IFsNodeFactory
    {
        Either<DirectoryNotFoundException, IDirectory> InstantiateDirectory(string path);
        IDirectory InstantiateDirectory(IDirectoryInfo info);
        IFile InstantiateFile(string path);
        IFile InstantiateFile(IFileInfo info);
    }
}
