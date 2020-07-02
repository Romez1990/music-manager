using System;

namespace Core.FileSystem
{
    public interface IFsNodeElementFactory
    {
        IDirectoryElement CreateDirectoryElement(
            string path,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler);

        IDirectoryElement CreateDirectoryElementInsideDirectory(
            IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler);

        IFileElement CreateFileElementInsideDirectory(
            IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler);
    }
}
