using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectory>, IDirectoryElement
    {
        public DirectoryElement(IDirectory directory, EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler) : base(directory, uncheckHandler, checkHandler)
        {
            CheckedPartiallyEvent += checkPartiallyHandler;
        }

        public DirectoryElement(IDirectory directory) : base(directory)
        {
        }

        public ImmutableArray<IFsNodeElement<IFsNode>> Content { get; }

        private event EventHandler<FsNodeElementCheckEventArgs> CheckedPartiallyEvent;

        private void OnCheckedPartiallyEvent()
        {
            CheckedPartiallyEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        public override void Uncheck()
        {
            base.Check();
            Content.ToList().ForEach(fsNodeElement => fsNodeElement.Uncheck());
        }

        public override void Check()
        {
            base.Check();
            Content.ToList().ForEach(fsNodeElement => fsNodeElement.Check());
        }

        private void ContentUncheckHandler(object sender, FsNodeElementCheckEventArgs e)
        {
            if (Content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Unchecked))
            {
                CheckState = CheckState.Unchecked;
                OnUncheckEvent();
            }
            else if (CheckState != CheckState.CheckedPartially)
            {
                CheckState = CheckState.CheckedPartially;
                OnCheckedPartiallyEvent();
            }
        }

        private void ContentCheckPartiallyHandler(object sender, FsNodeElementCheckEventArgs e)
        {
            if (CheckState != CheckState.CheckedPartially)
            {
                CheckState = CheckState.CheckedPartially;
                OnCheckedPartiallyEvent();
            }
        }
  
        private void ContentCheckHandler(object sender, FsNodeElementCheckEventArgs e)
        {
            if (Content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Checked))
            {
                CheckState = CheckState.Checked;
                OnCheckEvent();
            }
            else if (CheckState == CheckState.Unchecked)
            {
                CheckState = CheckState.CheckedPartially;
                OnCheckedPartiallyEvent();
            }
        }
    }
}
