using System;
using System.Collections.Generic;
using Core.IocContainer;

namespace Core.FileSystem {
    [Service]
    public class NaturalStringComparerFactory : INaturalStringComparerFactory {
        public IComparer<string> Create() {
            if (OperatingSystem.IsWindows())
                return new WindowsNaturalStringComparer();
            throw new PlatformNotSupportedException();
        }
    }
}
