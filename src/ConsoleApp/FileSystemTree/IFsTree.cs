using Core.FileSystem;

namespace ConsoleApp.FileSystemTree
{
    public interface IFsTree
    {
        IDirectoryElement DirectoryElement { set; }

        string ToString();
    }
}
