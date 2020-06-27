using System;

namespace Core.FileSystem
{
    public class FileElement : FsNodeElementBase<IFile>, IFileElement
    {
        public FileElement(IFile file, EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler) : base(file, checkHandler, uncheckHandler)
        {
        }
    }
}
