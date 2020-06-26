using System;

namespace Core.FileSystem
{
    public class FileElement : FsNodeElementBase, IFileElement
    {
        public FileElement(IFile file, EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler) : base(file, checkHandler, uncheckHandler)
        {
            FsNode = file;
        }

        public FileElement(IFile file) : base(file)
        {
            FsNode = file;
        }

        public new IFile FsNode { get; }
    }
}
