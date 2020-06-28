using Core.CoreEngine;
using Core.FileSystem;

namespace Core.Operation
{
    public interface IOperation
    {
        void Perform(IDirectoryElement directoryElement, Mode mode);
    }
}
