using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.FileSystem.Exceptions;
using LanguageExt;
using SystemPath = System.IO.Path;
using SystemDirectoryNotFoundException = System.IO.DirectoryNotFoundException;

namespace Core.FileSystem {
    public class Directory : FsNodeBase<DirectoryInfo>, IDirectory {
        public Directory(IFsNodeFactory fsNodeFactory, IComparer<string> naturalStringComparer, DirectoryInfo info,
            ChildrenRetrieval<IFsNode> childrenRetrieval)
            : base(info) {
            _fsNodeFactory = fsNodeFactory;
            _naturalStringComparer = naturalStringComparer;
            _children = new Lazy<IReadOnlyList<IFsNode>>(() => GetChildren(childrenRetrieval));
        }

        private readonly IFsNodeFactory _fsNodeFactory;

        public override string ParentPath => Info.Parent?.FullName ?? throw new InvalidOperationException();

        private readonly Lazy<IReadOnlyList<IFsNode>> _children;
        public IReadOnlyList<IFsNode> Children => _children.Value;

        private IReadOnlyList<IFsNode> GetChildren(ChildrenRetrieval<IFsNode> childrenRetrieval) =>
            childrenRetrieval.Retrieve(CreateChildren, TakeChildren);

        private readonly IComparer<string> _naturalStringComparer;

        private IReadOnlyList<IFsNode> CreateChildren() {
            var directories = Info.GetDirectories()
                .Map(info => _fsNodeFactory.CreateDirectoryFromInfo(info, ChildrenRetrieval<IFsNode>.Create()))
                .OrderBy(fsNode => fsNode.Name, _naturalStringComparer);
            var files = Info.GetFiles()
                .Map(info => _fsNodeFactory.CreateFileFromInfo(info))
                .OrderBy(fsNode => fsNode.Name, _naturalStringComparer);
            return directories
                .Cast<IFsNode>()
                .Concat(files)
                .Map(SubscribeChild)
                .ToArray();
        }

        private IReadOnlyList<IFsNode> TakeChildren(IEnumerable<IFsNode> children) =>
            children
                .Map(ResubscribeChild)
                .ToArray();

        private IFsNode SubscribeChild(IFsNode fsNode) {
            fsNode.Changed += OnChildChanged;
            return fsNode;
        }

        private IFsNode UnsubscribeChild(IFsNode fsNode) {
            fsNode.Unsubscribe();
            return fsNode;
        }

        private IFsNode ResubscribeChild(IFsNode fsNode) =>
            SubscribeChild(UnsubscribeChild(fsNode));

        private void OnChildChanged(object sender, FsNodeChangedEventArgs e) {
            var oldFsNode = (IFsNode)sender;
            var newFsNode = e.FsNode;
            var newChildren = Children
                .Map(fsNode => ReferenceEquals(fsNode, oldFsNode) ? newFsNode : fsNode)
                .ToArray();
            var newDirectory =
                _fsNodeFactory.CreateDirectoryFromInfo(Info, ChildrenRetrieval<IFsNode>.Take(newChildren));
            InvokeChanged(newDirectory);
        }

        public Either<FsException, IDirectory> Rename(string newName) {
            var newPath = SystemPath.Combine(ParentPath, newName);
            return MoveDirectoryInfo(newPath, newName)
                .Map(newInfo => {
                    var newDirectory =
                        _fsNodeFactory.CreateDirectoryFromInfo(newInfo, ChildrenRetrieval<IFsNode>.Create());
                    InvokeChanged(newDirectory);
                    return newDirectory;
                });
        }

        private Either<FsException, DirectoryInfo> MoveDirectoryInfo(string newPath, string newName) {
            var newInfo = new DirectoryInfo(Path);
            try {
                newInfo.MoveTo(newPath);
                return newInfo;
            } catch (SystemDirectoryNotFoundException) {
                return new Exceptions.DirectoryNotFoundException(Name);
            } catch (IOException e) {
                var message = e.Message;
                if (message.StartsWith("Cannot create") &&
                    message.EndsWith("because a file or directory with the same name already exists.")) {
                    return new DirectoryAlreadyExistsException(newName);
                }

                if (message.StartsWith("Access to the path") && message.EndsWith("is denied.")) {
                    return new DirectoryAccessDeniedException(Name);
                }

                // TODO: have to log this exception
                return new UnknownDirectoryException(message);
            }
        }

        public override string ToString() =>
            $"Directory {{ Name = {Name} }}";
    }
}
