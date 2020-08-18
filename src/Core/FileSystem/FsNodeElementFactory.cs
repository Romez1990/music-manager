using System.IO;
using LanguageExt;

namespace Core.FileSystem
{
    public class FsNodeElementFactory : IFsNodeElementFactory
    {
        public FsNodeElementFactory(IFsNodeFactory fsNodeFactory)
        {
            _fsNodeFactory = fsNodeFactory;
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        public Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path)
        {
            return _fsNodeFactory.InstantiateDirectory(path)
                .Map(directory => (IDirectoryElement)new DirectoryElement(this, directory));
        }

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory)
        {
            return new DirectoryElement(this, directory);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file)
        {
            return new FileElement(file);
        }
    }
}
