using Core.FileSystem;

namespace Core.CoreEngine
{
    public interface IEngineFactory
    {
        IEngineScanner CreateEngineScanner(IDirectoryElement directory);
        IEnginePerformer CreateEnginePerformer(IDirectoryElement directory, Mode mode);
    }
}
