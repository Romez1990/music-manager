using System.IO;
using System.IO.Abstractions;

namespace Core.FileSystem
{
    public class FsInfoFactory : IFsInfoFactory
    {
        public IFileInfo CreateFileInfo(string path)
        {
            return (FileInfoBase)new FileInfo(path);
        }

        public IDirectoryInfo CreateDirectoryInfo(string path)
        {
            return (DirectoryInfoBase)new DirectoryInfo(path);
        }
    }
}
