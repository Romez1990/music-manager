using Core.FileSystem;
using Core.IocContainer;

namespace Core.FileSystemElement {
    [Service]
    public class FsNodeElementFactory : IFsNodeElementFactory {
        private const CheckState StartingCheckState = CheckState.Unchecked;

        public IFileElement CreateFileElementFromFile(IFile file) =>
            CreateFileElementFromFile(file, StartingCheckState);

        public IFileElement CreateFileElementFromFile(IFile file, CheckState checkState) =>
            new FileElement(this, file, checkState);
    }
}
