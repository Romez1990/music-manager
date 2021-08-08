using System.Collections.Generic;

namespace Core.FileSystem {
    public interface INaturalStringComparerFactory {
        IComparer<string> Create();
    }
}
