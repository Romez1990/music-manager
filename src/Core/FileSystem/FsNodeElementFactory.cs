using System;
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

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler)
        {
            return new DirectoryElement(this, directory, uncheckHandler, checkPartiallyHandler, checkHandler);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler)
        {
            return new FileElement(file, uncheckHandler, checkHandler);
        }
    }
}
