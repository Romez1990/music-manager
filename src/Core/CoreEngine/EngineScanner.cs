using Core.FileScanner;
using Core.FileSystem;

namespace Core.CoreEngine
{
    public class EngineScanner : IEngineScanner
    {
        internal EngineScanner(IEngineFactory engineFactory, IScanner scanner, IDirectoryElement directoryElement)
        {
            _engineFactory = engineFactory;
            _scanner = scanner;
            _directoryElement = directoryElement;
        }

        private readonly IEngineFactory _engineFactory;
        private readonly IScanner _scanner;
        private readonly IDirectoryElement _directoryElement;

        public IEnginePerformer Scan(Mode mode)
        {
            var newDirectoryElement = _scanner.Scan(_directoryElement, mode);
            return _engineFactory.CreateEnginePerformer(newDirectoryElement, mode);
        }
    }
}
