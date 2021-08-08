using System.Collections.Generic;
using NativeLibraries.Windows;

namespace Core.FileSystem {
    public class WindowsNaturalStringComparer : IComparer<string> {
        public int Compare(string a, string b) =>
            ShlWApi.StrCmpLogicalW(a, b);
    }
}
