using System;
using System.Collections.Generic;
using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystemElement {
    public interface IDirectoryElement : IFsNodeElement {
        IReadOnlyList<IFsNodeElement> Children { get; }
        Either<FsException, IDirectoryElement> Rename(string newName);
        IDirectoryElement Uncheck(bool ignoreIfUnchecked = false);
        IDirectoryElement Check(bool ignoreIfChecked = false);
        IDirectoryElement SetCheckState(CheckState checkState, bool ignoreIfSameState = false);
        bool IsExpanded { get; }
        IDirectoryElement Expand();
        IDirectoryElement Collapse();
        event EventHandler<RootDirectoryElementChangedEventArgs> RootChanged;
        void UnsubscribeRoot();
    }
}
