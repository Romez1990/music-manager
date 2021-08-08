using System.Collections.Generic;
using System.IO;
using Core.IocContainer;

namespace Core.FileSystem {
    [Service]
    public class FsNodeFactory : IFsNodeFactory {
        public FsNodeFactory(INaturalStringComparerFactory naturalStringComparerFactory) {
            _naturalStringComparer = naturalStringComparerFactory.Create();
        }

        private readonly IComparer<string> _naturalStringComparer;

        public IDirectory CreateDirectoryFromInfo(DirectoryInfo info, ChildrenRetrieval<IFsNode> childrenRetrieval) =>
            new Directory(this, _naturalStringComparer, info, childrenRetrieval);

        public IFile CreateFileFromInfo(FileInfo info) =>
            new File(this, info);
    }
}
