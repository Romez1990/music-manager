using Core.FileSystem;
using Core.FileSystem.Exceptions;
using LanguageExt;

namespace Core.CoreEngine
{
    public class Engine : IEngine
    {
        public Engine(IEngineFactory engineFactory, IFsNodeElementFactory fsNodeElementFactory)
        {
            _engineFactory = engineFactory;
            _fsNodeElementFactory = fsNodeElementFactory;
        }

        private readonly IEngineFactory _engineFactory;
        private readonly IFsNodeElementFactory _fsNodeElementFactory;

        public Either<DirectoryNotFoundException, IEngineScanner> SetDirectory(string path)
        {
            return _fsNodeElementFactory.CreateDirectoryElement(path)
                .Map(directory => _engineFactory.CreateEngineScanner(directory));
        }
    }
}
