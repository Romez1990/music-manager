using Core.FileSystem;

namespace ConsoleApp.FileSystemTree
{
    public interface IFsTree
    {
        IDirectoryElement DirectoryElement { get; set; }
    }
}
