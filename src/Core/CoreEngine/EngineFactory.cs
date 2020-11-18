using Core.FileScanner;
using Core.FileSystem;
using Core.Operations.Operation;

namespace Core.CoreEngine
{
    public class EngineFactory : IEngineFactory
    {
        public EngineFactory(IScanner scanner, IOperationRepository operationRepository)
        {
            _scanner = scanner;
            _operationRepository = operationRepository;
        }

        private readonly IScanner _scanner;
        private readonly IOperationRepository _operationRepository;

        public IEngineScanner CreateEngineScanner(IDirectoryElement directory)
        {
            return new EngineScanner(this, _scanner, directory);
        }

        public IEnginePerformer CreateEnginePerformer(IDirectoryElement directory, Mode mode)
        {
            return new EnginePerformer(_operationRepository, directory, mode);
        }
    }
}
