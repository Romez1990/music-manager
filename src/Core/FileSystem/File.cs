using System.IO.Abstractions;
using IO = System.IO;

namespace Core.FileSystem
{
    public class File : FsNodeBase<IFileInfo>, IFile
    {
        public File(IFsInfoFactory fsInfoFactory, IFileInfo info) : base(info)
        {
            _fsInfoFactory = fsInfoFactory;
        }

        private readonly IFsInfoFactory _fsInfoFactory;

        public string Extension => Info.Extension;

        public IFile Rename(string newName)
        {
            var newInfo = _fsInfoFactory.CreateFileInfo(Path);
            var newPath = IO.Path.Combine(Info.DirectoryName, newName);
            newInfo.MoveTo(newPath);
            return new File(_fsInfoFactory, newInfo);
        }
    }
}
