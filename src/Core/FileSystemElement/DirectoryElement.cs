using System;
using System.Collections.Generic;
using System.Linq;
using Core.FileSystem;
using Core.FileSystem.Exceptions;
using Core.FileSystemElement.Exceptions;
using LanguageExt;

namespace Core.FileSystemElement {
    public class DirectoryElement : FsNodeElementBase<IDirectory>, IDirectoryElement {
        public DirectoryElement(IFsNodeElementFactory fsNodeElementFactory, IDirectory directory, CheckState checkState,
            bool isExpanded, ChildrenRetrieval<IFsNodeElement> childrenRetrieval)
            : base(directory, checkState) {
            _fsNodeElementFactory = fsNodeElementFactory;
            IsExpanded = isExpanded;
            _children = new Lazy<IReadOnlyList<IFsNodeElement>>(() => GetChildren(childrenRetrieval));
        }

        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        private readonly Lazy<IReadOnlyList<IFsNodeElement>> _children;
        public IReadOnlyList<IFsNodeElement> Children => _children.Value;

        private IReadOnlyList<IFsNodeElement> GetChildren(ChildrenRetrieval<IFsNodeElement> childrenRetrieval) =>
            childrenRetrieval.Retrieve(CreateChildren, TakeChildren, TakeChildrenState);

        private IReadOnlyList<IFsNodeElement> CreateChildren() =>
            FsNode.Children
                .Map(CreateFsNodeElement)
                .Map(SubscribeChild)
                .ToArray();

        private IReadOnlyList<IFsNodeElement> TakeChildren(IEnumerable<IFsNodeElement> children) =>
            children
                .Map(ResubscribeChild)
                .ToArray();

        private IReadOnlyList<IFsNodeElement> TakeChildrenState(IEnumerable<IFsNodeElement> oldChildren) =>
            oldChildren
                .Map(UnsubscribeChild)
                .Zip(FsNode.Children)
                .Map(TakeFsNodeElementState)
                .Map(SubscribeChild)
                .ToArray();

        private IFsNodeElement CreateFsNodeElement(IFsNode fsNode) =>
            fsNode.Match<IFsNodeElement>(
                _fsNodeElementFactory.CreateDirectoryElementFromDirectory,
                _fsNodeElementFactory.CreateFileElementFromFile
            );

        private IFsNodeElement TakeFsNodeElementState((IFsNodeElement, IFsNode) tuple) {
            var (fsNodeElement, fsNode) = tuple;
            return fsNodeElement.Match<IFsNodeElement>(
                directoryElement =>
                    _fsNodeElementFactory.CreateDirectoryElementFromDirectory((IDirectory)fsNode,
                        directoryElement.CheckState, directoryElement.IsExpanded,
                        ChildrenRetrieval<IFsNodeElement>.TakeStateOnly(directoryElement.Children)),
                fileElement =>
                    _fsNodeElementFactory.CreateFileElementFromFile((IFile)fsNode, fileElement.CheckState)
            );
        }

        private IFsNodeElement SubscribeChild(IFsNodeElement fsNodeElement) {
            fsNodeElement.Changed += OnChildChanged;
            return fsNodeElement;
        }

        private IFsNodeElement UnsubscribeChild(IFsNodeElement fsNodeElement) {
            fsNodeElement.Unsubscribe();
            return fsNodeElement;
        }

        private IFsNodeElement ResubscribeChild(IFsNodeElement fsNodeElement) =>
            SubscribeChild(UnsubscribeChild(fsNodeElement));

        private bool _ignoreChildrenChanged;

        private void OnChildChanged(object sender, FsNodeElementChangedEventArgs e) {
            if (_ignoreChildrenChanged) return;

            var oldFsNode = (IFsNodeElement)sender;
            var newFsNode = e.FsNodeElement;
            var newChildren = Children
                .Map(fsNode => ReferenceEquals(fsNode, oldFsNode) ? newFsNode : fsNode)
                .ToArray();
            var newCheckState = oldFsNode.CheckState != newFsNode.CheckState
                ? ChangeCheckState(newChildren)
                : CheckState;
            var newDirectory = _fsNodeElementFactory.CreateDirectoryElementFromDirectory(FsNode, newCheckState,
                IsExpanded, ChildrenRetrieval<IFsNodeElement>.Take(newChildren));
            InvokeChanged(newDirectory);
        }

        private CheckState ChangeCheckState(IReadOnlyCollection<IFsNodeElement> newChildren) =>
            CheckState switch {
                CheckState.Unchecked =>
                    newChildren.All(fsNode => fsNode.CheckState is CheckState.Checked)
                        ? CheckState.Checked
                        : CheckState.CheckedPartially,
                CheckState.CheckedPartially =>
                    newChildren.All(fsNode => fsNode.CheckState is CheckState.Checked)
                        ? CheckState.Checked
                        : newChildren.All(fsNode => fsNode.CheckState is CheckState.Unchecked)
                            ? CheckState.Unchecked
                            : CheckState.CheckedPartially,
                CheckState.Checked =>
                    newChildren.All(fsNode => fsNode.CheckState is CheckState.Unchecked)
                        ? CheckState.Unchecked
                        : CheckState.CheckedPartially,
                _ => throw new NotSupportedException(),
            };

        public Either<FsException, IDirectoryElement> Rename(string newName) =>
            FsNode.Rename(newName)
                .Map(newDirectory => {
                    var newDirectoryElement = _fsNodeElementFactory.CreateDirectoryElementFromDirectory(newDirectory,
                        CheckState, IsExpanded, ChildrenRetrieval<IFsNodeElement>.TakeStateOnly(Children));
                    InvokeChanged(newDirectoryElement);
                    return newDirectoryElement;
                });

        public IDirectoryElement Uncheck(bool ignoreIfUnchecked = false) =>
            SetCheckState(CheckState.Unchecked, ignoreIfUnchecked);

        public IDirectoryElement Check(bool ignoreIfChecked = false) =>
            SetCheckState(CheckState.Checked, ignoreIfChecked);

        public IDirectoryElement SetCheckState(CheckState checkState, bool ignoreIfSameState = false) {
            if (checkState == CheckState) {
                if (!ignoreIfSameState)
                    throw new CheckStateException(checkState);
                return this;
            }

            _ignoreChildrenChanged = true;
            var newChildren = Children
                .Map(fsNode => SetFsNodeCheckState(fsNode, checkState))
                .ToArray();
            _ignoreChildrenChanged = false;
            var newDirectory = _fsNodeElementFactory.CreateDirectoryElementFromDirectory(FsNode, checkState, IsExpanded,
                ChildrenRetrieval<IFsNodeElement>.Take(newChildren));
            InvokeChanged(newDirectory);
            return newDirectory;
        }

        private IFsNodeElement SetFsNodeCheckState(IFsNodeElement fsNode, CheckState checkState) =>
            fsNode.Match<IFsNodeElement>(
                directory => directory.SetCheckState(checkState, true),
                file => file.SetCheckState(checkState, true)
            );

        public bool IsExpanded { get; }

        public IDirectoryElement Expand() =>
            SetIsExpanded(true);

        public IDirectoryElement Collapse() =>
            SetIsExpanded(false);

        private IDirectoryElement SetIsExpanded(bool isExpanded) {
            if (isExpanded == IsExpanded)
                throw new IsExpandedException(isExpanded);
            _ignoreChildrenChanged = true;
            var newChildren = isExpanded
                ? Children
                : CollapseChildren(Children);
            _ignoreChildrenChanged = false;
            var newDirectory = _fsNodeElementFactory.CreateDirectoryElementFromDirectory(FsNode, CheckState, isExpanded,
                ChildrenRetrieval<IFsNodeElement>.Take(newChildren));
            InvokeChanged(newDirectory);
            return newDirectory;
        }

        private IReadOnlyList<IFsNodeElement> CollapseChildren(IEnumerable<IFsNodeElement> children) =>
            children
                .Map(fsNode => fsNode.MatchDirectory(directory => directory.Expand()))
                .ToArray();

        public override string ToString() =>
            $"DirectoryElement {{ Name = {Name}, CheckState = {CheckState}, IsExpanded = {IsExpanded} }}";
    }
}
