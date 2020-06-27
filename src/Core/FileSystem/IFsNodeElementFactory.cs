using System;

namespace Core.FileSystem
{
    public interface IFsNodeElementFactory
    {
        IDirectoryElement CreateDirectoryElement(string path);

        IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler);

        IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler);
    }
}
