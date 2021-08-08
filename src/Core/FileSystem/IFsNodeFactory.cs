using System.IO;

namespace Core.FileSystem {
    public interface IFsNodeFactory {
        IFile CreateFileFromInfo(FileInfo info);
    }
}
