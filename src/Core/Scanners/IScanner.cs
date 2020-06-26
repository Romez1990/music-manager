using Core.Engines;
using Core.FileSystem;

namespace Core.Scanners
{
    public interface IScanner
    {
        void Scan(IDirectoryElement directoryElement, Mode mode);
    }
}
