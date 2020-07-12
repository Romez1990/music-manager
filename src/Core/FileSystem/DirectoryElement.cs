using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectory>, IDirectoryElement
    {
        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkPartiallyHandler,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler) : base(directory, uncheckHandler, checkHandler)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            CheckedPartiallyEvent += checkPartiallyHandler;
            Content = GetContent();
        }

        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory) : base(directory)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = GetContent();
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public override void Rename(string newName)
        {
            FsNode.Rename(newName);
        }

        public ImmutableArray<IFsNodeElement> Content { get; }

        private ImmutableArray<IFsNodeElement> GetContent()
        {
            return FsNode.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Select(directory => _fsNodeElementFactory.CreateDirectoryElementInsideDirectory(directory,
                    ContentUncheckHandler, ContentCheckPartiallyHandler, ContentCheckHandler))
                .Cast<IFsNodeElement>()
                .Concat(FsNode.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Select(file =>
                        _fsNodeElementFactory.CreateFileElementInsideDirectory(file, ContentUncheckHandler,
                            ContentCheckHandler)))
                .ToImmutableArray();
        }

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
