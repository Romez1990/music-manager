using System;

namespace Core.FileSystem
{
    public class FileElement : FsNodeElementBase<IFile>, IFileElement
    {
        public FileElement(IFile file, EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler) : base(file, uncheckHandler, checkHandler)
        {
        }
    }
}
