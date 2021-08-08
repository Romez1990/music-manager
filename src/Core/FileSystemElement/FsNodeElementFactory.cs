using Core.FileSystem;
using Core.IocContainer;

namespace Core.FileSystemElement {
    [Service]
    public class FsNodeElementFactory : IFsNodeElementFactory {
        private const CheckState StartingCheckState = CheckState.Unchecked;

        public IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory) =>
            throw new System.NotImplementedException();

        public IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory, CheckState checkState,
            bool isExpanded, ChildrenRetrieval<IFsNodeElement> childrenRetrieval) =>
            throw new System.NotImplementedException();

        public IFileElement CreateFileElementFromFile(IFile file) =>
            CreateFileElementFromFile(file, StartingCheckState);

        public IFileElement CreateFileElementFromFile(IFile file, CheckState checkState) =>
            new FileElement(this, file, checkState);
    }
}
