using Core.FileSystem;
using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.FileSystemElement {
    public interface IFsNodeElementFactory {
        Either<DirectoryNotFoundException, IDirectoryElement> CreateDirectoryElement(string path);
        Either<FileNotFoundException, IFileElement> CreateFileElement(string path);
        IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory);

        IDirectoryElement CreateDirectoryElementFromDirectory(IDirectory directory, CheckState checkState,
            bool isExpanded, ChildrenRetrieval<IFsNodeElement> childrenRetrieval);

        IFileElement CreateFileElementFromFile(IFile file);
        IFileElement CreateFileElementFromFile(IFile file, CheckState checkState);
    }
}
