using Core.CoreEngine;
using Core.FileSystem;

namespace Core.Operation
{
    public interface IOperation
    {
        string Name { get; }
        string Description { get; }
        IDirectoryElement Perform(IDirectoryElement directoryElement, Mode mode);
    }
}
