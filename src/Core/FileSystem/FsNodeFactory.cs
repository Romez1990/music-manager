using System.IO.Abstractions;

namespace Core.FileSystem
{
    public class FsNodeFactory : IFsNodeFactory
    {
        public FsNodeFactory(IFsInfoFactory fsInfoFactory)
        {
            _fsInfoFactory = fsInfoFactory;
        }

        private readonly IFsInfoFactory _fsInfoFactory;

        public IDirectory InstantiateDirectory(string path)
        {
            return new Directory(this, _fsInfoFactory, _fsInfoFactory.CreateDirectoryInfo(path));
        }

        public IDirectory InstantiateDirectory(IDirectoryInfo info)
        {
            return new Directory(this, _fsInfoFactory, info);
        }

        public IFile InstantiateFile(string path)
        {
            return new File(_fsInfoFactory, _fsInfoFactory.CreateFileInfo(path));
        }

        public IFile InstantiateFile(IFileInfo info)
        {
            return new File(_fsInfoFactory, info);
        }
    }
}
