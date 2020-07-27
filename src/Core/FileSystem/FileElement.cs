using System;

namespace Core.FileSystem
{
    public class FileElement : FsNodeElementBase<IFileElement, IFile>, IFileElement
    {
        public FileElement(IFile file, EventHandler<CheckStateChangeEventArgs> checkStateChange) :
            this(file, checkStateChange, CheckState.Unchecked)
        {
        }

        private FileElement(IFile file, EventHandler<CheckStateChangeEventArgs> checkStateChange,
            CheckState checkState)
            : base(file, checkStateChange, checkState)
        {
        }

        public string Extension => FsNode.Extension;

        public override IFileElement Rename(string newName)
        {
            var newFile = FsNode.Rename(newName);
            return new FileElement(newFile, CheckStateChangeHandler, CheckState);
        }

        public override IFileElement Uncheck()
        {
            return new FileElement(FsNode, CheckStateChangeHandler, CheckState.Unchecked);
        }

        public override IFileElement Check()
        {
            return new FileElement(FsNode, CheckStateChangeHandler, CheckState.Checked);
        }
    }
}
