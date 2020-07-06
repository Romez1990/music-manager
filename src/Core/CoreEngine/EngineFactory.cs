using Core.FileScanner;
using Core.FileSystem;
using Core.Operation;

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

        public IEngineScanner CreateEngineScanner(IDirectoryElement directoryElement)
        {
            return new EngineScanner(this, _scanner, directoryElement);
        }

        public IEnginePerformer CreateEnginePerformer(IDirectoryElement directoryElement, Mode mode)
        {
            return new EnginePerformer(_operationRepository, directoryElement, mode);
        }
    }
}
