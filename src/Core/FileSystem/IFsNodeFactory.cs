using System.IO;

namespace Core.FileSystem {
    public interface IFsNodeFactory {
        IDirectory CreateDirectoryFromInfo(DirectoryInfo info, ChildrenRetrieval<IFsNode> childrenRetrieval);
        IFile CreateFileFromInfo(FileInfo info);
    }
}
