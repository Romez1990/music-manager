using Core.CoreEngine;
using Core.FileSystem;

namespace Core.Operation
{
    public interface IOperation
    {
        string Description { get; }

        void Perform(IDirectoryElement directoryElement, Mode mode);
    }
}
