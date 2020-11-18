using Core.CoreEngine;
using Core.FileSystem;

namespace Core.FileScanner
{
    public interface IScanner
    {
        IDirectoryElement Scan(IDirectoryElement directory, Mode mode);
    }
}
