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
            CheckState checkState, ImmutableArray<IFsNodeElement<object>> content)
            : base(directory, checkState)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            Content = content;
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public override IDirectoryElement Rename(string newName)
        {
            var newDirectory = FsNode.Rename(newName);
            return new DirectoryElement(_fsNodeElementFactory, newDirectory, CheckState);
        }

        public ImmutableArray<IFsNodeElement<object>> Content { get; }

        private ImmutableArray<IFsNodeElement<object>> GetContent()
        {
            return FsNode.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Select(directory => new DirectoryElement(_fsNodeElementFactory, directory))
                .Cast<IFsNodeElement<object>>()
                .Concat(FsNode.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Select(file => new FileElement(file)))
                .ToImmutableArray();
        }

        public override IDirectoryElement Uncheck()
        {
            var newContent = Content
                .Select(fsNodeElement => (IFsNodeElement<object>)fsNodeElement.Uncheck())
                .ToImmutableArray();
            return new DirectoryElement(_fsNodeElementFactory, FsNode, CheckState.Unchecked, newContent);
        }

        public override IDirectoryElement Check()
        {
            var newContent = Content
                .Select(fsNodeElement => (IFsNodeElement<object>)fsNodeElement.Check())
                .ToImmutableArray();
            return new DirectoryElement(_fsNodeElementFactory, FsNode, CheckState.Checked, newContent);
        }

        public IDirectoryElement SelectContent(Func<IFsNodeElement<object>, IFsNodeElement<object>> selector)
        {
            var newContent = Content.Select(selector).ToImmutableArray();
            var checkState = DefineCheckState(newContent);
            return new DirectoryElement(_fsNodeElementFactory, FsNode, checkState, newContent);
        }

        public IDirectoryElement SelectContent(Func<IFsNodeElement<object>, int, IFsNodeElement<object>> selector)
        {
            var newContent = Content.Select(selector).ToImmutableArray();
            var checkState = DefineCheckState(newContent);
            return new DirectoryElement(_fsNodeElementFactory, FsNode, checkState, newContent);
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
