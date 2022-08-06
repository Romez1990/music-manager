using System.Collections.Immutable;
using Core.FileSystemElement;
using Core.IocContainer;
using Core.OperationEngine;
using Core.Renamer;

namespace Core.Operation {
    [Service(ToSelf = true)]
    public class RenameOperation : IOperation {
        public RenameOperation(IRenamer renamer) {
            _renamer = renamer;
        }

        public string Name => "Rename";

        public string Description => "Rename track files and album folders";

        private readonly IRenamer _renamer;

        public OperationResult Perform(IDirectoryElement directory, DirectoryMode directoryMode) {
            var renamingResult = _renamer.Rename(directory, directoryMode);
            return new OperationResult(renamingResult.Directory, renamingResult.Exceptions
                .Map(fsException => new OperationException(fsException))
                .ToImmutableList());
        }
    }
}
