using System.IO;
using Core.IocContainer;

namespace Core.FileSystem {
    [Service]
    public class FsNodeFactory : IFsNodeFactory {
        public IDirectory CreateDirectoryFromInfo(DirectoryInfo info, ChildrenRetrieval<IFsNode> childrenRetrieval) =>
            throw new System.NotImplementedException();

        public IFile CreateFileFromInfo(FileInfo info) =>
            new File(this, info);
    }
}
