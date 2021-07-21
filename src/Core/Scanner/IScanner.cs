using Core.FileSystemElement;
using Core.OperationEngine;

namespace Core.Scanner {
    public interface IScanner {
        IDirectoryElement Scan(IDirectoryElement directory, DirectoryMode directoryMode);
    }
}
