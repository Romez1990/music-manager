using System.IO.Abstractions;

namespace Core.FileSystem
{
    public class FsNodeFactory : IFsNodeFactory
    {
        public FsNodeFactory(IFileSystemInfoFactory fileSystemInfoFactory)
        {
            _fileSystemInfoFactory = fileSystemInfoFactory;
        }

        private readonly IFileSystemInfoFactory _fileSystemInfoFactory;

        public IFile InstantiateFile(string path)
        {
            return new File(_fileSystemInfoFactory, path);
        }

        public IFile InstantiateFile(IFileInfo info)
        {
            return new File(info);
        }

        public IDirectory InstantiateDirectory(string path)
        {
            return new Directory(_fileSystemInfoFactory, path);
        }

        public IDirectory InstantiateDirectory(IDirectoryInfo info)
        {
            return new Directory(info);
        }
    }
}
