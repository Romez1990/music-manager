using System.Collections.Generic;
using Core.Actions;
using Core.FileSystem;

namespace Core.Engines
{
    public interface IEngine
    {
        IDirectoryElement DirectoryElement { get; }

        bool SetDirectory(string path);

        void Scan(Mode mode);

        void PerformActions(IEnumerable<Action> actions);
    }
}
