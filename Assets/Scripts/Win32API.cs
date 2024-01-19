using System;
using System.Runtime.InteropServices;
using System.Text;
public class Win32API
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

    [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
    public static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
     [DllImport("user32.dll")]
    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CreateWindowEx(
        int dwExStyle,
        string lpClassName,
        string lpWindowName,
        int dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyWindow(IntPtr hWnd);

    // 창 생성 함수
    public static IntPtr CreateNewWindow(int width, int height, string title)
    {
        const int WS_OVERLAPPEDWINDOW = 0x00CF0000;
        const int CW_USEDEFAULT = -2147483648;

        IntPtr hWnd = CreateWindowEx(0, "static", title, WS_OVERLAPPEDWINDOW,
            CW_USEDEFAULT, CW_USEDEFAULT, width, height, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

        if (hWnd == IntPtr.Zero)
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }

        return hWnd;
    }

    public static void DestroyExistingWindow(IntPtr hWnd)
    {
        if (!DestroyWindow(hWnd))
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static void ShowWindowExample(IntPtr hWnd, int nCmdShow)
    {
        if (!ShowWindow(hWnd, nCmdShow))
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);

    public const int SW_HIDE = 0;
    public const int SW_SHOW = 5;
    public const int SW_MINIMIZE = 6;
    public const int SW_RESTORE = 9;

    [StructLayout(LayoutKind.Sequential)]
    public struct WNDCLASS
    {
        public uint style;
        public IntPtr lpfnWndProc; // 윈도우 프로시저
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
    }

    // RegisterClass 함수 선언
    [DllImport("user32.dll", SetLastError = true)]
    public static extern ushort RegisterClass([In] ref WNDCLASS lpWndClass);

    // DefWindowProc 함수 선언
    [DllImport("user32.dll")]
    public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")]
    public static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hDC);

    [DllImport("user32.dll")]
    public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BITMAPINFO pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        public RGBQUAD bmiColors;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth, biHeight;
        public ushort biPlanes, biBitCount;
        public uint biCompression, biSizeImage;
        public int biXPelsPerMeter, biYPelsPerMeter;
        public uint biClrUsed, biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public const uint SWP_NOSIZE = 0x0001;
    public const uint SWP_NOMOVE = 0x0002;
    public const uint SWP_SHOWWINDOW = 0x0040;

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, uint wFlags);

    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(POINT Point);

    public const int SRCCOPY = 0x00CC0020;

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBQUAD
    {
        public byte rgbBlue, rgbGreen, rgbRed, rgbReserved;
    }

    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
    public const int WS_OVERLAPPED = 0x00000000;
    public const int WS_CAPTION = 0x00C00000;
    public const int WS_SYSMENU = 0x00080000;
    public const int WS_THICKFRAME = 0x00040000;
    public const int WS_MINIMIZEBOX = 0x00020000;
    public const int WS_MAXIMIZEBOX = 0x00010000;
    public const int WS_BORDER = 0x00800000;
    public const int WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX;
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    public delegate IntPtr DefWindowProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")]
    public static extern void SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);
    [DllImport("user32.dll")]
    public static extern int GetSystemMetrics(int nIndex);
    [DllImport("gdi32.dll", SetLastError = true)]
    public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, IntPtr lpvBits);
    public static int gcd(int a, int b)
    {
        while(b != 0)
        {
            int r = a % b;
            a =b;
            b = r;
        }

        return a;
    }
}
