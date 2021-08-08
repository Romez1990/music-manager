using Core.FileSystem;

namespace Core.FileSystemElement {
    public interface IFsNodeElementFactory {
        IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory);

        IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory, CheckState checkState,
            bool isExpanded, ChildrenRetrieval<IFsNodeElement> childrenRetrieval);

        IFileElement CreateFileElementFromFile(IFile file);
        IFileElement CreateFileElementFromFile(IFile file, CheckState checkState);
    }
}
