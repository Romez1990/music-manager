using System.Linq;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operations.Operation;
using Core.Renaming;

namespace Core.Operations.Rename
{
    public class RenameOperation : IOperation
    {
        public RenameOperation(IRenamer renamer)
        {
            _renamer = renamer;
        }

        private readonly IRenamer _renamer;

        public string Name { get; } = "Rename";

        public string Description { get; } = "Rename tracks and album folders";

        public OperationResult Perform(IDirectoryElement directory, Mode mode)
        {
            var newDirectory = _renamer.Rename(directory, mode);
            return new OperationResult(newDirectory, Enumerable.Empty<OperationException>());
        }
    }
}
