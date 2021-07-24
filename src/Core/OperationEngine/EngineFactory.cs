using System;
using Core.FileSystemElement;
using Core.IocContainer;

namespace Core.OperationEngine {
    [Service]
    public class EngineFactory : IEngineFactory {
        public IEngineScanner CreateScanner(IDirectoryElement directory) =>
            throw new NotImplementedException();
    }
}
