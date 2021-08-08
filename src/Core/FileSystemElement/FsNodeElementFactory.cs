using Core.FileSystem;
using Core.IocContainer;

namespace Core.FileSystemElement {
    [Service]
    public class FsNodeElementFactory : IFsNodeElementFactory {
        public IFileElement CreateFileElementFromFile(IFile file) =>
            throw new System.NotImplementedException();

        public IFileElement CreateFileElementFromFile(IFile file, CheckState checkState) =>
            throw new System.NotImplementedException();
    }
}
