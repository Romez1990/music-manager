using Core.FileSystem;

namespace Core.CoreEngine
{
    public interface IEngineFactory
    {
        IEngineScanner CreateEngineScanner(IDirectoryElement directoryElement);
        IEnginePerformer CreateEnginePerformer(IDirectoryElement directoryElement, Mode mode);
    }
}
