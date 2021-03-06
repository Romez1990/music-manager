using System.IO.Abstractions;
using Core.FileSystem.Exceptions;
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
            return info.Exists switch
            {
                false => Left(new DirectoryNotFoundException(path)),
                true => new Directory(_fsInfoFactory, this, info),
            };
        }

        public IDirectory InstantiateDirectory(IDirectoryInfo info) =>
            new Directory(_fsInfoFactory, this, info);

        public IFile InstantiateFile(string path) =>
            new File(_fsInfoFactory, _fsInfoFactory.CreateFileInfo(path));

        public IFile InstantiateFile(IFileInfo info) =>
            new File(_fsInfoFactory, info);
    }
}
