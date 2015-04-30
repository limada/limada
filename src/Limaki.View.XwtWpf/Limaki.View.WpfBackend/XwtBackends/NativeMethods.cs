using System;
using System.Runtime.InteropServices;

namespace Gecko {
    internal static class NativeMethods {
        #region Constants

        public const int WM_MOUSEDOWN = 0x0210;
        public const int WM_HOVER = 0x20;
        #endregion

        #region Delegates and Structs

        public delegate IntPtr WndProc (IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public static readonly WndProc DefaultWindowProc = DefWindowProc;

        [StructLayout (LayoutKind.Sequential)]
        public struct TRACKMOUSEEVENT {
            public int cbSize;
            public uint dwFlags;
            public IntPtr hWnd;
            public uint dwHoverTime;
        }

        [StructLayout (LayoutKind.Sequential)]
        public struct WNDCLASSEX {
            public uint cbSize;
            public uint style;
            [MarshalAs (UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;
        }

        [StructLayout (LayoutKind.Sequential)]
        public struct NativePoint {
            public int X;
            public int Y;
        }

        #endregion

        #region DllImports

        [DllImport ("user32.dll")]
        public extern static int GetWindowLong (IntPtr hwnd, int index);

        [DllImport ("user32.dll")]
        public extern static int SetWindowLong (IntPtr hwnd, int index, int value);

        [DllImport ("user32.dll")]
        public extern static bool SetWindowPos (IntPtr hwnd, IntPtr hwndInsertAfter,
            int x, int y, int width, int height, uint flags);

        [DllImport ("user32.dll")]
        public static extern IntPtr SendMessage (IntPtr hwnd, uint msg,
            IntPtr wParam, IntPtr lParam);

        [DllImport ("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateWindowEx (
            int exStyle,
            string className,
            string windowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            [MarshalAs (UnmanagedType.AsAny)] object pvParam);

        [DllImport ("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Auto)]
        public static extern bool DestroyWindow (IntPtr hwnd);

        [DllImport ("user32.dll")]
        public static extern IntPtr DefWindowProc (IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport ("kernel32.dll")]
        public static extern IntPtr GetModuleHandle (string module);

        [DllImport ("user32.dll")]
        public static extern IntPtr LoadCursor (IntPtr hInstance, int lpCursorName);

        [DllImport ("user32.dll")]
        public static extern int TrackMouseEvent (ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport ("user32.dll")]
        [return: MarshalAs (UnmanagedType.U2)]
        public static extern short RegisterClassEx ([In] ref WNDCLASSEX lpwcx);

        [DllImport ("user32.dll")]
        public static extern int ScreenToClient (IntPtr hWnd, ref NativePoint pt);

        [DllImport ("user32.dll")]
        public static extern int SetFocus (IntPtr hWnd);

        [DllImport ("user32.dll")]
        public static extern IntPtr GetFocus ();

        [DllImport ("user32.dll")]
        public static extern IntPtr SetCapture (IntPtr hWnd);

        [DllImport ("user32.dll")]
        public static extern bool ReleaseCapture ();

        [DllImport ("user32.dll")]
        public static extern bool GetCursorPos (ref NativePoint point);

        [DllImport ("user32.dll")]
        public static extern bool SetCursorPos (int x, int y);

        [DllImport ("user32.dll")]
        public static extern int ShowCursor (bool bShow);

        #endregion

        #region Helpers

        public static int GetXLParam (int lParam) {
            return LowWord (lParam);
        }

        public static int GetYLParam (int lParam) {
            return HighWord (lParam);
        }

        public static int GetWheelDeltaWParam (int wParam) {
            return HighWord (wParam);
        }

        public static int LowWord (int input) {
            return (short)(input & 0xffff);
        }

        public static int HighWord (int input) {
            return (short)(input >> 16);
        }

        #endregion
    }
}