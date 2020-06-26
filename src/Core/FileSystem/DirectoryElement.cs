using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase, IDirectoryElement
    {
        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory,
            IDirectory directory, EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler) : base(directory, checkHandler, uncheckHandler)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            FsNode = directory;
            Content = SelectContent();
            CheckedPartiallyEvent += checkPartiallyHandler;
        }

        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory,
            IDirectory directory) : base(directory)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            FsNode = directory;
            Content = SelectContent();
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public new IDirectory FsNode { get; }

        public ImmutableArray<IFsNodeElement> Content { get; }

        private ImmutableArray<IFsNodeElement> SelectContent()
        {
            return FsNode.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Select(directory => _fsNodeElementFactory.CreateDirectoryElementInsideDirectory(directory,
                    ContentCheckHandler, ContentCheckPartiallyHandler, ContentUncheckHandler))
                .Cast<IFsNodeElement>()
                .Concat(FsNode.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Select(file => _fsNodeElementFactory.CreateFileElementInsideDirectory(file,
                        ContentCheckHandler, ContentUncheckHandler)))
                .ToImmutableArray();
        }

        public event EventHandler<FsNodeElementCheckEventArgs> CheckedPartiallyEvent;

        private void OnCheckedPartiallyEvent()
        {
            CheckedPartiallyEvent?.Invoke(this, new FsNodeElementCheckEventArgs(CheckState));
        }

        public override void Check()
        {
            base.Check();
            Content.ToList().ForEach(fsNodeElement => fsNodeElement.Check());
        }

        public override void Uncheck()
        {
            base.Check();
            Content.ToList().ForEach(fsNodeElement => fsNodeElement.Uncheck());
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

        private void ContentCheckPartiallyHandler(object sender, FsNodeElementCheckEventArgs e)
        {
            if (CheckState != CheckState.CheckedPartially)
            {
                CheckState = CheckState.CheckedPartially;
                OnCheckedPartiallyEvent();
            }
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
    }
}
