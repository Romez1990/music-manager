using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectoryElement, IDirectory>, IDirectoryElement
    {
        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory)
            : this(fsNodeElementFactory, directory, CheckState.Unchecked)
        {
        }

        private DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory,
            CheckState checkState)
            : base(directory, checkState)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = GetContent();
        }

        private DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory,
            CheckState checkState, ImmutableArray<IFsNodeElement<object>> content, ContentAction contentAction)
            : base(directory, checkState)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = contentAction switch
            {
                ContentAction.Set => content,
                ContentAction.SetStateOnly => GetContent(content),
                _ => throw new ArgumentOutOfRangeException(nameof(contentAction), contentAction, null),
            };
        }

        private enum ContentAction
        {
            Set,
            SetStateOnly,
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public override IDirectoryElement Rename(string newName)
        {
            var newDirectory = FsNode.Rename(newName);
            return new DirectoryElement(_fsNodeElementFactory, newDirectory, CheckState, Content,
                ContentAction.SetStateOnly);
        }

        public ImmutableArray<IFsNodeElement<object>> Content { get; }

        private ImmutableArray<IFsNodeElement<object>> GetContent() =>
            FsNode.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Map(directory => new DirectoryElement(_fsNodeElementFactory, directory))
                .Cast<IFsNodeElement<object>>()
                .Concat(FsNode.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Map(file => new FileElement(file)))
                .ToImmutableArray();

        private ImmutableArray<IFsNodeElement<object>> GetContent(ImmutableArray<IFsNodeElement<object>> content) =>
            FsNode.Content
                .Zip(content)
                .Where(((IFsNode<object> fsNode, IFsNodeElement<object> fsNodeElement) t) => t.fsNode is IDirectory)
                .Map(((IFsNode<object> fsNode, IFsNodeElement<object> fsNodeElement) t) =>
                    ((IDirectory)t.fsNode, (IDirectoryElement)t.fsNodeElement))
                .Map(((IDirectory directory, IDirectoryElement directoryElement) t) =>
                    new DirectoryElement(_fsNodeElementFactory, t.directory, t.directoryElement.CheckState,
                        t.directoryElement.Content, ContentAction.SetStateOnly))
                .Cast<IFsNodeElement<object>>()
                .Concat(FsNode.Content
                    .Zip(content)
                    .Where(((IFsNode<object> fsNode, IFsNodeElement<object> fsNodeElement) t) => t.fsNode is IFile)
                    .Map(((IFsNode<object> fsNode, IFsNodeElement<object> fsNodeElement) t) =>
                        ((IFile)t.fsNode, (IFileElement)t.fsNodeElement))
                    .Map(((IFile file, IFileElement fileElement) t) =>
                        new FileElement(t.file, t.fileElement.CheckState)))
                .ToImmutableArray();

        public override IDirectoryElement Uncheck()
        {
            var newContent = Content
                .Map(fsNodeElement => (IFsNodeElement<object>)fsNodeElement.Uncheck())
                .ToImmutableArray();
            return new DirectoryElement(_fsNodeElementFactory, FsNode, CheckState.Unchecked, newContent,
                ContentAction.Set);
        }

        public override IDirectoryElement Check()
        {
            var newContent = Content
                .Map(fsNodeElement => (IFsNodeElement<object>)fsNodeElement.Check())
                .ToImmutableArray();
            return new DirectoryElement(_fsNodeElementFactory, FsNode, CheckState.Checked, newContent,
                ContentAction.Set);
        }

        public IDirectoryElement MapContent(Func<IFsNodeElement<object>, IFsNodeElement<object>> function)
        {
            var newContent = Content
                .Map(function)
                .ToImmutableArray();

            var checkState = DefineCheckState(newContent);
            return new DirectoryElement(_fsNodeElementFactory, FsNode, checkState, newContent, ContentAction.Set);
        }

        public IDirectoryElement MapContent(Func<int, IFsNodeElement<object>, IFsNodeElement<object>> function)
        {
            var newContent = Content
                .Map(function)
                .ToImmutableArray();

            var checkState = DefineCheckState(newContent);
            return new DirectoryElement(_fsNodeElementFactory, FsNode, checkState, newContent, ContentAction.Set);
        }

        private CheckState DefineCheckState(ImmutableArray<IFsNodeElement<object>> content)
        {
            if (content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Unchecked))
                return CheckState.Unchecked;
            if (content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Checked))
                return CheckState.Checked;
            return CheckState.CheckedPartially;
        }
    }
}
