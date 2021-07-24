using Core.Scanner;
using Core.FileSystemElement;
using Core.IocContainer;
using Core.Operation;

namespace Core.OperationEngine {
    [Service]
    public class EngineFactory : IEngineFactory {
        public EngineFactory(IScanner scanner, IOperationRepository operations) {
            _scanner = scanner;
            _operations = operations;
        }

        private readonly IScanner _scanner;
        private readonly IOperationRepository _operations;

        public IEngineScanner CreateScanner(IDirectoryElement directory) =>
            new EngineScanner(this, _scanner, directory);

        public IEnginePerformer CreateEnginePerformer(IDirectoryElement directory, DirectoryMode directoryMode) =>
            new EnginePerformer(_operations, directory, directoryMode);
    }
}
