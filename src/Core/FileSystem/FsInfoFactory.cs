using System.IO;
using System.IO.Abstractions;

namespace Core.FileSystem
{
    public class FsInfoFactory : IFsInfoFactory
    {
        public IFileInfo CreateFileInfo(string path) =>
            (FileInfoBase)new FileInfo(path);

        public IDirectoryInfo CreateDirectoryInfo(string path) =>
            (DirectoryInfoBase)new DirectoryInfo(path);
    }
}
