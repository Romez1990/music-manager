using System;
using System.IO;
using LanguageExt;

namespace Core.FileSystem
{
    public interface IFsNodeElementFactory
    {
        Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path);

        IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler);

        IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler);
    }
}
