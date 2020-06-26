using System.IO.Abstractions;

namespace Core.FileSystem
{
    public interface IFsNodeFactory
    {
        IDirectory InstantiateDirectory(string path);

        IDirectory InstantiateDirectory(IDirectoryInfo info);

        IFile InstantiateFile(string path);

        IFile InstantiateFile(IFileInfo info);
    }
}
