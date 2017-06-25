using System;
using System.Runtime.InteropServices;

namespace WindowWatcher
{
    public static class User32
    {
        [DllImport("User32")]
        public static extern bool SetProcessDPIAware();

        [DllImport("User32")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("User32")]
        public static extern IntPtr GetParent(IntPtr hwnd);

        [DllImport("User32")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, out IntPtr lpdwProcessId);

        [DllImport("User32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hwnd);

        [DllImport("User32")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        #region Nested type: LASTINPUTINFO

        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)] public int cbSize;
            [MarshalAs(UnmanagedType.U4)] public int dwTime;
        }

        #endregion
    }
}