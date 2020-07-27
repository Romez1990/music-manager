using System;
using System.IO;
using LanguageExt;

namespace Core.FileSystem
{
    public interface IFsNodeElementFactory
    {
        Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path);

        IDirectoryElement CreateDirectoryElementInsideDirectory(IDirectory directory,
            EventHandler<CheckStateChangeEventArgs> uncheckHandler,
            EventHandler<CheckStateChangeEventArgs> checkPartiallyHandler,
            EventHandler<CheckStateChangeEventArgs> checkHandler);

        IFileElement CreateFileElementInsideDirectory(IFile file,
            EventHandler<CheckStateChangeEventArgs> uncheckHandler,
            EventHandler<CheckStateChangeEventArgs> checkHandler);
    }
}
