using Core.FileSystem;

namespace ConsoleApp.FileSystemTree
{
    public interface IFsTreePrinter
    {
        string Print(IDirectoryElement directory);
    }
}
