using System.Collections.Generic;
using System.Linq;
using Core.FileSystem;
using Core.Operation;

namespace Core.CoreEngine
{
    public class EnginePerformer : IEnginePerformer
    {
        internal EnginePerformer(IDirectoryElement directoryElement, Mode mode)
        {
            DirectoryElement = directoryElement;
            _mode = mode;
        }

        public IDirectoryElement DirectoryElement { get; }

        private readonly Mode _mode;

        public void PerformOperations(IEnumerable<IOperation> operations)
        {
            operations
                .ToList()
                .ForEach(operation => operation.Perform(DirectoryElement, _mode));
        }
    }
}
