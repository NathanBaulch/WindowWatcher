using System;
using System.Runtime.InteropServices;

namespace WindowWatcher
{
    public static class Shell32
    {
        [DllImport("Shell32")]
        public static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion, out IntPtr piSmallVersion, int amountIcons);
    }
}