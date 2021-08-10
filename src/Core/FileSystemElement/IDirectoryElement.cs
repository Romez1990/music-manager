using System;
using System.Collections.Generic;
using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystemElement {
    public interface IDirectoryElement : IFsNodeElement {
        IReadOnlyList<IFsNodeElement> Children { get; }
        IDirectoryElement MapChildren(Func<IFsNodeElement, IFsNodeElement> mapper);
        IDirectoryElement MapChildren(Func<int, IFsNodeElement, IFsNodeElement> mapper);
        IDirectoryElement MapDirectories(Func<IDirectoryElement, IDirectoryElement> mapper);
        IDirectoryElement MapDirectories(Func<int, IDirectoryElement, IDirectoryElement> mapper);
        IDirectoryElement MapFiles(Func<IFileElement, IFileElement> mapper);
        IDirectoryElement MapFiles(Func<int, IFileElement, IFileElement> mapper);
        Either<FsException, IDirectoryElement> Rename(string newName);
        IDirectoryElement Uncheck(bool ignoreIfUnchecked = false);
        IDirectoryElement Check(bool ignoreIfChecked = false);
        IDirectoryElement SetCheckState(CheckState checkState, bool ignoreIfSameState = false);
        IDirectoryElement IfChecked(Func<IDirectoryElement> onChecked);
        bool IsExpanded { get; }
        IDirectoryElement Expand();
        IDirectoryElement Collapse();
        event EventHandler<RootDirectoryElementChangedEventArgs> RootChanged;
        void UnsubscribeRoot();
    }
}
