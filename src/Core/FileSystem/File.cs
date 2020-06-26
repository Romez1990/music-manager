using System.IO.Abstractions;
using Path = System.IO.Path;

namespace Core.FileSystem
{
    public class File : FsNodeBase<IFileInfo>, IFile
    {
        public File(IFileSystemInfoFactory fileSystemInfoFactory, string path) :
            base(fileSystemInfoFactory.CreateFileInfo(path))
        {
        }

        public File(IFileInfo info) : base(info)
        {
        }

        public override void Rename(string newName)
        {
            var newPath = Path.Combine(Info.DirectoryName, newName);
            Info.MoveTo(newPath);
        }

        public string DirectoryName => Info.DirectoryName;

        public string Extension => Info.Extension;
    }
}
