using System;
using System.Collections.Immutable;
using System.Linq;

namespace Core.FileSystem
{
    public class DirectoryElement : FsNodeElementBase<IDirectory>, IDirectoryElement
    {
        private DirectoryElement(
            IFsNodeElementFactory fsNodeElementFactory,
            IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler,
            ImmutableArray<IFsNodeElement<IFsNode>> content,
            CheckState checkState) :
            base(directory, checkStateChangeHandler, checkState)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            _directory = directory;
            Content = content;
        }

        private DirectoryElement(
            IFsNodeElementFactory fsNodeElementFactory,
            IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler,
            CheckState checkState) :
            base(directory, checkStateChangeHandler, checkState)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            _directory = directory;
            Content = GetContent();
        }

        public DirectoryElement(
            IFsNodeElementFactory fsNodeElementFactory,
            IDirectory directory,
            EventHandler<FsNodeElementCheckEventArgs> checkStateChangeHandler) :
            base(directory, checkStateChangeHandler)
        {
            _fsNodeElementFactory = fsNodeElementFactory;
            _directory = directory;
            Content = GetContent();
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        private readonly IDirectory _directory;

        public ImmutableArray<IFsNodeElement<IFsNode>> Content { get; }

        private ImmutableArray<IFsNodeElement<IFsNode>> GetContent()
        {
            return _directory.Content
                .Where(fsNode => fsNode is IDirectory)
                .Cast<IDirectory>()
                .Select(directory =>
                    _fsNodeElementFactory.CreateDirectoryElementInsideDirectory(directory,
                        ContentCheckStateChangeHandler))
                .Cast<IFsNodeElement<IFsNode>>()
                .Concat(_directory.Content
                    .Where(fsNode => fsNode is IFile)
                    .Cast<IFile>()
                    .Select(file => _fsNodeElementFactory.CreateFileElementInsideDirectory(file, null)))
                .ToImmutableArray();
        }

        public IDirectoryElement Rename(string newName)
        {
            return new DirectoryElement(
                _fsNodeElementFactory,
                _directory.Rename(newName),
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

        public IDirectoryElement UncheckSilently()
        {
            var newDirectoryElements = Content
                .Where(fsNodeElement => fsNodeElement is IDirectoryElement)
                .Cast<IDirectoryElement>()
                .Select(directoryElement => directoryElement.UncheckSilently());
            var newFileElements = Content
                .Where(fsNodeElement => fsNodeElement is IFileElement)
                .Cast<IFileElement>()
                .Select(fileElement => fileElement.UncheckSilently());
            var newContent = newDirectoryElements
                .Cast<IFsNodeElement<IFsNode>>()
                .Concat(newFileElements)
                .ToImmutableArray();
            return new DirectoryElement(
                _fsNodeElementFactory,
                _directory,
                CheckStateChangeHandler,
                newContent,
                CheckState.Unchecked
            );
        }

        public IDirectoryElement CheckSilently()
        {
            var newDirectoryElements = Content
                .Where(fsNodeElement => fsNodeElement is IDirectoryElement)
                .Cast<IDirectoryElement>()
                .Select(directoryElement => directoryElement.CheckSilently());
            var newFileElements = Content
                .Where(fsNodeElement => fsNodeElement is IFileElement)
                .Cast<IFileElement>()
                .Select(fileElement => fileElement.CheckSilently());
            var newContent = newDirectoryElements
                .Cast<IFsNodeElement<IFsNode>>()
                .Concat(newFileElements)
                .ToImmutableArray();
            return new DirectoryElement(
                _fsNodeElementFactory,
                _directory,
                CheckStateChangeHandler,
                newContent,
                CheckState.Checked
            );
        }

        private void ContentCheckStateChangeHandler(object sender, FsNodeElementCheckEventArgs e)
        {
            var oldFsNodeElement = (IFsNodeElement<IFsNode>)sender;
            var newFsNodeElement = e.FsNodeElement;
            var newDirectoryElement = ChangeDirectoryElement(oldFsNodeElement, newFsNodeElement);
            OnCheckStateChange(newDirectoryElement);
        }

        private IDirectoryElement ChangeDirectoryElement(IFsNodeElement<IFsNode> oldChildFsNodeElement,
            IFsNodeElement<IFsNode> newChildFsNodeElement)
        {
            var newContent = Content.Replace(oldChildFsNodeElement, newChildFsNodeElement);
            var checkState = ResolveCheckState(newContent, newChildFsNodeElement.CheckState);
            return new DirectoryElement(
                _fsNodeElementFactory,
                _directory,
                CheckStateChangeHandler,
                newContent,
                checkState
            );
        }

        private CheckState ResolveCheckState(ImmutableArray<IFsNodeElement<IFsNode>> content,
            CheckState childCheckState)
        {
            return childCheckState switch
            {
                CheckState.Unchecked => content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Unchecked)
                    ? CheckState.Unchecked
                    : CheckState.CheckedPartially,
                CheckState.CheckedPartially => CheckState.CheckedPartially,
                CheckState.Checked => content.All(fsNodeElement => fsNodeElement.CheckState == CheckState.Checked)
                    ? CheckState.Checked
                    : CheckState.CheckedPartially,
                _ => throw new ArgumentOutOfRangeException(nameof(childCheckState), childCheckState, null),
            };
        }
    }
}
