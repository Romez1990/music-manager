using Core.Scanner;
using Core.FileSystemElement;

namespace Core.OperationEngine {
    public class EngineScanner : IEngineScanner {
        public EngineScanner(IEngineFactory engineFactory, IScanner scanner, IDirectoryElement directory) {
            _engineFactory = engineFactory;
            _scanner = scanner;
            _directory = directory;
        }

        private readonly IEngineFactory _engineFactory;
        private readonly IScanner _scanner;
        private readonly IDirectoryElement _directory;

        public IEnginePerformer Scan(DirectoryMode directoryMode) {
            var newDirectory = _scanner.Scan(_directory, directoryMode);
            return _engineFactory.CreateEnginePerformer(newDirectory, directoryMode);
        }
    }
}
