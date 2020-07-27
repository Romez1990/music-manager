using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectoryElement, IDirectory>, IDirectoryElement
    {
        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory,
            EventHandler<CheckStateChangeEventArgs> uncheck,
            EventHandler<CheckStateChangeEventArgs> checkPartiallyHandler,
            EventHandler<CheckStateChangeEventArgs> checkHandler) : base(directory, uncheck, checkHandler)
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

        public override IDirectoryElement Rename(string newName)
        {
            FsNode.Rename(newName);
            throw new NotImplementedException();
        }

        public ImmutableArray<IFsNodeElement<object>> Content { get; }

        private ImmutableArray<IFsNodeElement<object>> GetContent()
        {
            return FsNode.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Select(directory => _fsNodeElementFactory.CreateDirectoryElementInsideDirectory(directory,
                    ContentUncheckHandler, ContentCheckPartiallyHandler, ContentCheckHandler))
                .Cast<IFsNodeElement<object>>()
                .Concat(FsNode.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Select(file =>
                        _fsNodeElementFactory.CreateFileElementInsideDirectory(file, ContentUncheckHandler,
                            ContentCheckHandler))
                    .Cast<IFsNodeElement<object>>())
                .ToImmutableArray();
        }

        private event EventHandler<CheckStateChangeEventArgs> CheckedPartiallyEvent;

        private void OnCheckedPartiallyEvent()
        {
            CheckedPartiallyEvent?.Invoke(this, new CheckStateChangeEventArgs(CheckState));
        }

        public override IDirectoryElement Uncheck()
        {
            base.Check();
            Content.ToList().ForEach(fsNodeElement => fsNodeElement.Uncheck());
            throw new NotImplementedException();
        }

        public override IDirectoryElement Check()
        {
            base.Check();
            Content.ToList().ForEach(fsNodeElement => fsNodeElement.Check());
            throw new NotImplementedException();
        }

        private void ContentUncheckHandler(object sender, CheckStateChangeEventArgs e)
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

        private void ContentCheckPartiallyHandler(object sender, CheckStateChangeEventArgs e)
        {
            if (CheckState != CheckState.CheckedPartially)
            {
                CheckState = CheckState.CheckedPartially;
                OnCheckedPartiallyEvent();
            }
        }

        private void ContentCheckHandler(object sender, CheckStateChangeEventArgs e)
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
