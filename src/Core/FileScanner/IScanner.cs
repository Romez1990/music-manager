using Core.CoreEngine;
using Core.FileSystem;

namespace Core.FileScanner
{
    public interface IScanner
    {
        void Scan(IDirectoryElement directoryElement, Mode mode);
    }
}
