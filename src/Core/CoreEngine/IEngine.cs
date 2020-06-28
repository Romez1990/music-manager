using System.Collections.Generic;
using Core.FileSystem;
using Core.Operation;

namespace Core.CoreEngine
{
    public interface IEngine
    {
        IDirectoryElement DirectoryElement { get; }

        bool SetDirectory(string path);

        void Scan(Mode mode);

        void PerformActions(IEnumerable<IOperation> operations);
    }
}
