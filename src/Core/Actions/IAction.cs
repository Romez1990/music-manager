using Core.Engines;
using Core.FileSystem;

namespace Core.Actions
{
    public interface IAction
    {
        void Perform(IDirectoryElement directoryElement, Mode mode);
    }
}
