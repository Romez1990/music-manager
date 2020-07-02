using System;

namespace Core.FileSystem
{
    public class FileElement : FsNodeElementBase<IFile>, IFileElement
    {
        private FileElement(
            IFile file,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler,
            CheckState checkState) :
            base(file, checkStateChangeHandler, checkState)
        {
        }

        public FileElement(IFile file, EventHandler<FsNodeElementCheckEventArgs> checkStateChange) :
            base(file, checkStateChange)
        {
        }

        public IFileElement Rename(string newName)
        {
            return new FileElement(
                FsNode.Rename(newName),
                CheckStateChangeHandler,
                CheckState
            );
        }

        public override void Uncheck()
        {
            OnCheckStateChange(UncheckSilently());
        }

        public override void Check()
        {
            OnCheckStateChange(CheckSilently());
        }

        public IFileElement UncheckSilently()
        {
            return new FileElement(
                FsNode,
                CheckStateChangeHandler,
                CheckState.Unchecked
            );
        }

        public IFileElement CheckSilently()
        {
            return new FileElement(
                FsNode,
                CheckStateChangeHandler,
                CheckState.Checked
            );
        }
    }
}
