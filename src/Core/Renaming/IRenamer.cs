using Core.CoreEngine;
using Core.FileSystem;

namespace Core.Renaming
{
    public interface IRenamer
    {
        IDirectoryElement Rename(IDirectoryElement directory, Mode mode);
    }
}
