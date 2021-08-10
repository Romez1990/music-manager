using System;
using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystemElement {
    public interface IFileElement : IFsNodeElement {
        string Extension { get; }
        Either<FsException, IFileElement> Rename(string newName);
        IFileElement Uncheck(bool ignoreIfUnchecked = false);
        IFileElement Check(bool ignoreIfChecked = false);
        IFileElement SetCheckState(CheckState checkState, bool ignoreIfSameState = false);
        IFileElement IfChecked(Func<IFileElement> onChecked);
    }
}
