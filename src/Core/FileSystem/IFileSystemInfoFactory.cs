using System.IO.Abstractions;

namespace Core.FileSystem
{
    public interface IFileSystemInfoFactory
    {
        IDirectoryInfo CreateDirectoryInfo(string path);

        IFileInfo CreateFileInfo(string path);
    }
}
