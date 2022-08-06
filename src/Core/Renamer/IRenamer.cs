using Core.FileSystemElement;
using Core.OperationEngine;

namespace Core.Renamer {
    public interface IRenamer {
        RenamingResult Rename(IDirectoryElement directory, DirectoryMode mode);
    }
}
