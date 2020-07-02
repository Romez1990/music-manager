using System.Collections.Immutable;

namespace Core.FileSystem
{
    public interface IDirectory : IFsNode
    {
        ImmutableArray<IFsNode> Content { get; }
   
        IDirectory Rename(string newName);
    }
}
