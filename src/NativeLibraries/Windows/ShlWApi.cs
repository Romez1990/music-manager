using System.Runtime.InteropServices;

namespace NativeLibraries.Windows {
    public static class ShlWApi {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);
    }
}
