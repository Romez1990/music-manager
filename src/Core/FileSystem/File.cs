using System.IO.Abstractions;
using IO = System.IO;

namespace Core.FileSystem
{
    public class File : FsNodeBase<IFileInfo>, IFile
    {
        public File(IFileInfo info) : base(info)
        {
        }

        public string Extension => Info.Extension;

        public override void Rename(string newName)
        {
            var newPath = IO.Path.Combine(Info.DirectoryName, newName);
            Info.MoveTo(newPath);
        }
    }
}
