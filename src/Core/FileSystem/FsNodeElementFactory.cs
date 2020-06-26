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

        public IFileElement CreateFileElement(string path)
        {
            return new FileElement(_fsNodeFactory.InstantiateFile(path));
        }

        public IFileElement CreateFileElement(IFile file)
        {
            return new FileElement(file);
        }

        public IDirectoryElement CreateDirectoryElement(string path)
        {
            return new DirectoryElement(this, _fsNodeFactory.InstantiateDirectory(path));
        }

        public IDirectoryElement CreateDirectoryElement(IDirectory directory)
        {
            return new DirectoryElement(this, directory);
        }

        public IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler)
        {
            return new FileElement(file, checkHandler, uncheckHandler);
        }

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler)
        {
            return new DirectoryElement(this, directory, checkHandler, checkPartiallyHandler, uncheckHandler);
        }
    }
}
