using Core.FileScanner;
using Core.FileSystem;

namespace Core.CoreEngine
{
    public class EngineFactory : IEngineFactory
    {
        public EngineFactory(IScanner scanner)
        {
            _scanner = scanner;
        }

        private readonly IScanner _scanner;

        public IEngineScanner CreateEngineScanner(IDirectoryElement directoryElement)
        {
            return new EngineScanner(this, _scanner, directoryElement);
        }

        public IEnginePerformer CreateEnginePerformer(IDirectoryElement directoryElement, Mode mode)
        {
            return new EnginePerformer(directoryElement, mode);
        }
    }
}
