using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectory>, IDirectoryElement
    {
        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkHandler,
            EventHandler<FsNodeElementCheckEventArgs> uncheckHandler) : base(directory, checkHandler, uncheckHandler)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = SelectContent();
        }

        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory) : base(directory)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = SelectContent();
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public ImmutableArray<IFsNodeElement<IFsNode>> Content { get; }

        private ImmutableArray<IFsNodeElement<IFsNode>> SelectContent()
        {
            return FsNode.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Select(directory => _fsNodeElementFactory.CreateDirectoryElementInsideDirectory(directory,
                    ContentCheckHandler, ContentUncheckHandler))
                .Cast<IFsNodeElement<IFsNode>>()
                .Concat(FsNode.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Select(file => _fsNodeElementFactory.CreateFileElementInsideDirectory(file,
                        ContentCheckHandler, ContentUncheckHandler)))
                .ToImmutableArray();
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
        }

        private void ContentUncheckHandler(object sender, FsNodeElementCheckEventArgs e)
        {
            if (Content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Unchecked))
            {
                CheckState = CheckState.Unchecked;
                OnUncheckEvent();
            }
        }
    }
}
