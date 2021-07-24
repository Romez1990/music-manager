using Core.FileSystem.Exceptions;
using Core.FileSystemElement;
using Core.IocContainer;
using LanguageExt;

namespace Core.OperationEngine {
    [Service]
    public class OperationEngine : IOperationEngine {
        public OperationEngine(IEngineFactory engineFactory, IFsNodeElementFactory fsNodeElementFactory) {
            _engineFactory = engineFactory;
            _fsNodeElementFactory = fsNodeElementFactory;
        }

        private readonly IEngineFactory _engineFactory;
        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public Either<DirectoryNotFoundException, IEngineScanner> SetDirectory(string path) =>
            _fsNodeElementFactory.CreateDirectoryElement(path)
                .Map(directory => _engineFactory.CreateScanner(directory));
    }
}
