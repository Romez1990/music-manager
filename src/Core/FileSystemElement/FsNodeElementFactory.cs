using Core.FileSystem;
using Core.IocContainer;

namespace Core.FileSystemElement {
    [Service]
    public class FsNodeElementFactory : IFsNodeElementFactory {
        private const CheckState StartingCheckState = CheckState.Unchecked;
        private const bool StartingIsExpanded = false;

        public IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory) =>
            CreateDirectoryElementFromDirectory(directory, StartingCheckState, StartingIsExpanded,
                ChildrenRetrieval<IFsNodeElement>.Create());

        public IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory, CheckState checkState,
            bool isExpanded, ChildrenRetrieval<IFsNodeElement> childrenRetrieval) =>
            new DirectoryElement(this, directory, checkState, isExpanded, childrenRetrieval);

        public IFileElement CreateFileElementFromFile(IFile file) =>
            CreateFileElementFromFile(file, StartingCheckState);

        public IFileElement CreateFileElementFromFile(IFile file, CheckState checkState) =>
            new FileElement(this, file, checkState);
    }
}
