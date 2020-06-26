using System;

namespace Core.FileSystem
{
    public interface IFsNodeElementFactory
    {
        IDirectoryElement CreateDirectoryElement(string path);

        IDirectoryElement CreateDirectoryElement(IDirectory directory);

        IFileElement CreateFileElement(string path);

        IFileElement CreateFileElement(IFile file);

        public IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler);

        public IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler);
    }
}
