using Core.CoreEngine;
using Core.FileSystem;

namespace Core.Operations.Operation
{
    public interface IOperation
    {
        string Name { get; }
        string Description { get; }
        IDirectoryElement Perform(IDirectoryElement directory, Mode mode);
    }
}
