using Core.FileSystem;

namespace ConsoleApp.FileSystemTree
{
    public interface IFsTreePrinter
    {
        void PrintTree(IDirectoryElement directoryElement);
    }
}
