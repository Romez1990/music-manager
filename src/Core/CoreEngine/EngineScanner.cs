using Core.FileScanner;
using Core.FileSystem;

namespace Core.CoreEngine
{
    public class EngineScanner : IEngineScanner
    {
        public EngineScanner(IEngineFactory engineFactory, IScanner scanner, IDirectoryElement directory)
        {
            _engineFactory = engineFactory;
            _scanner = scanner;
            _directory = directory;
        }

        private readonly IEngineFactory _engineFactory;
        private readonly IScanner _scanner;
        private readonly IDirectoryElement _directory;

        public IEnginePerformer Scan(Mode mode)
        {
            var newDirectory = _scanner.Scan(_directory, mode);
            return _engineFactory.CreateEnginePerformer(newDirectory, mode);
        }
    }
}
