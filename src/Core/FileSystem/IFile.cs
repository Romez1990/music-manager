using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystem {
    public interface IFile : IFsNode {
        string Extension { get; }
        Either<FsException, IFile> Rename(string newName);
    }
}
