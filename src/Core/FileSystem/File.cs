using System.IO.Abstractions;

namespace Core.FileSystem
{
    public class File : FsNodeBase<IFile, IFileInfo>, IFile
    {
        public File(IFsInfoFactory fsInfoFactory, IFileInfo info) : base(info)
        {
            _fsInfoFactory = fsInfoFactory;
        }

        private readonly IFsInfoFactory _fsInfoFactory;

        public string Extension => Info.Extension;

        public override IFile Rename(string newName)
        {
            var newPath = System.IO.Path.Combine(Info.DirectoryName, newName);
            var newInfo = _fsInfoFactory.CreateFileInfo(Path);
            newInfo.MoveTo(newPath);
            return new File(_fsInfoFactory, newInfo);
        }
    }
}
