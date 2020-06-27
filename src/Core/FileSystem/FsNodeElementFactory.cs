using System;

namespace Core.FileSystem
{
    public class FsNodeElementFactory : IFsNodeElementFactory
    {
        public FsNodeElementFactory(IFsNodeFactory fsNodeFactory)
        {
            _fsNodeFactory = fsNodeFactory;
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        public IDirectoryElement CreateDirectoryElement(string path)
        {
            return new DirectoryElement(this, _fsNodeFactory.InstantiateDirectory(path));
        }

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler)
        {
            return new DirectoryElement(this, directory, checkHandler, checkPartiallyHandler, uncheckHandler);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler)
        {
            return new FileElement(file, checkHandler, uncheckHandler);
        }
    }
}
