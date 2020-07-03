using System.Collections.Generic;

namespace Core.FileSystem
{
    public class FsNodeElementEqualityComparer : IEqualityComparer<IFsNodeElement<IFsNode>>
    {
        public bool Equals(IFsNodeElement<IFsNode> x, IFsNodeElement<IFsNode> y)
        {
            return x.FsNode == y.FsNode;
        }

        public int GetHashCode(IFsNodeElement<IFsNode> obj)
        {
            return obj.FsNode.GetHashCode();
        }
    }
}
