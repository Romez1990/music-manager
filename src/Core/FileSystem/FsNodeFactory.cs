using System.IO;
using System.IO.Abstractions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.FileSystem
{
    public class FsNodeFactory : IFsNodeFactory
    {
        public FsNodeFactory(IFsInfoFactory fsInfoFactory)
        {
            _fsInfoFactory = fsInfoFactory;
        }

        private readonly IFsInfoFactory _fsInfoFactory;

        public Either<DirectoryNotFoundException, IDirectory> InstantiateDirectory(string path)
        {
            var info = _fsInfoFactory.CreateDirectoryInfo(path);
            if (!info.Exists)
            {
                return Left(new DirectoryNotFoundException($"Directory {path} not found"));
            }

            return new Directory(this, info);
        }

        public IDirectory InstantiateDirectory(IDirectoryInfo info)
        {
            return new Directory(this, info);
        }

        public IFile InstantiateFile(string path)
        {
            return new File(_fsInfoFactory.CreateFileInfo(path));
        }

        public IFile InstantiateFile(IFileInfo info)
        {
            return new File(info);
        }
    }
}
