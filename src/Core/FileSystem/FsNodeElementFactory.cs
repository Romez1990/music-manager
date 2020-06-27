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
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler)
        {
            return new DirectoryElement(this, directory, uncheckHandler, checkHandler);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler)
        {
            return new FileElement(file, uncheckHandler, checkHandler);
        }
    }
}
