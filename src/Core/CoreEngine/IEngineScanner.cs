namespace Core.CoreEngine
{
    public interface IEngineScanner
    {
        IEnginePerformer Scan(Mode mode);
    }
}
