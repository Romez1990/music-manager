using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectoryElement : IFsNodeElement<IDirectory>
    {
        ImmutableArray<IFsNodeElement<IFsNode>> Content { get; }

        IDirectoryElement Rename(string newName);
        
        IDirectoryElement UncheckSilently();
    
        IDirectoryElement CheckSilently();
    }
}
