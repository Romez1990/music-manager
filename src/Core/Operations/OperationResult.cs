using System.Collections.Generic;
using Core.FileSystem;

namespace Core.Operations
{
    public class OperationResult
    {
        public OperationResult(IDirectoryElement directory, IEnumerable<OperationException> exceptions)
        {
            Directory = directory;
            Exceptions = exceptions;
        }

        public IDirectoryElement Directory { get; }
        public IEnumerable<OperationException> Exceptions { get; }
    }
}
