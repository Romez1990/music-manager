using System.Collections.Generic;
using Core.FileSystem.Exceptions;
using Core.FileSystemElement;

namespace Core.Renamer {
    public record RenamingResult(IDirectoryElement Directory, IReadOnlyList<FsException> Exceptions);
}
