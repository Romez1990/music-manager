using Core.Scanner;
using Core.FileSystemElement;
using Core.IocContainer;

namespace Core.OperationEngine {
    [Service]
    public class EngineFactory : IEngineFactory {
        public EngineFactory(IScanner scanner) {
            _scanner = scanner;
        }

        private readonly IScanner _scanner;

        public IEngineScanner CreateScanner(IDirectoryElement directory) =>
            new EngineScanner(this, _scanner, directory);

        public IEnginePerformer CreateEnginePerformer(IDirectoryElement directory, DirectoryMode directoryMode) =>
            throw new System.NotImplementedException();
    }
}
