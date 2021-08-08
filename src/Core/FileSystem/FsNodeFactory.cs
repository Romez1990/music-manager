using System.IO;
using Core.IocContainer;

namespace Core.FileSystem {
    [Service]
    public class FsNodeFactory : IFsNodeFactory {
        public IFile CreateFileFromInfo(FileInfo info) =>
            new File(this, info);
    }
}
