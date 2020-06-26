using System.IO.Abstractions;

namespace Core.FileSystem
{
    public interface IFsInfoFactory
    {
        IDirectoryInfo CreateDirectoryInfo(string path);

        IFileInfo CreateFileInfo(string path);
    }
}
