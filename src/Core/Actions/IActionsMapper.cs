using Core.Engines;
using Core.FileSystem;

namespace Core.Actions
{
    public interface IActionsMapper
    {
        void Perform(Action action, IDirectoryElement directoryElement, Mode mode);
    }
}
