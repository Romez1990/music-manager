using System.Collections.Generic;
using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystem {
    public interface IDirectory : IFsNode {
        IReadOnlyList<IFsNode> Children { get; }
        Either<FsException, IDirectory> Rename(string newName);
    }
}
