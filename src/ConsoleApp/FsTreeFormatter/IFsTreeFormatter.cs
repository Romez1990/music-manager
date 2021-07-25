using Core.FileSystemElement;

namespace ConsoleApp.FsTreeFormatter {
    public interface IFsTreeFormatter {
        string ToString(IDirectoryElement directory);
    }
}
