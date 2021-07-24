using Core.FileSystem.Exceptions;
using LanguageExt;
using DirectoryInfo = System.IO.DirectoryInfo;
using FileInfo = System.IO.FileInfo;

namespace Core.FileSystem {
    public interface IFsNodeFactory {
        Either<DirectoryNotFoundException, IDirectory> CreateDirectory(string path);
        Either<FileNotFoundException, IFile> CreateFile(string path);
        IDirectory CreateDirectoryFromInfo(DirectoryInfo info, ChildrenRetrieval<IFsNode> childrenRetrieval);
        IFile CreateFileFromInfo(FileInfo info);
    }
}
