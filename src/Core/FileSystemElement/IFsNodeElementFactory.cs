using Core.FileSystem;

namespace Core.FileSystemElement {
    public interface IFsNodeElementFactory {
        IFileElement CreateFileElementFromFile(IFile file);
        IFileElement CreateFileElementFromFile(IFile file, CheckState checkState);
    }
}
