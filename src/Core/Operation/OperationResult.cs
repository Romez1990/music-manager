using System.Collections.Immutable;
using Core.FileSystemElement;

namespace Core.Operation {
    public record OperationResult(IDirectoryElement Directory, ImmutableList<OperationException> Exceptions) {
        public static OperationResult operator +(OperationResult a, OperationResult b) =>
            new(b.Directory, a.Exceptions.AddRange(b.Exceptions));
    }
}
