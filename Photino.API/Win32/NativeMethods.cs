using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PhotinoAPI.Win32
{
    internal static class NativeMethods
    {
        public const uint FOS_PICKFOLDERS = 0x00000020U;
        public const uint FOS_FORCEFILESYSTEM = 0x00000040U;
        public const uint FOS_NOVALIDATE = 0x00000100U;
        public const uint FOS_NOTESTFILECREATE = 0x00010000U;
        public const uint FOS_DONTADDTORECENT = 0x02000000U;

        public const uint S_OK = 0x0000U;

        public const uint SIGDN_FILESYSPATH = 0x80058000U;

        public const int WM_SETICON = 0x80;
        public const int WM_NCHITTEST = 0x84;
        public const int WM_SYSCOMMAND = 0x0112;

        public const int GWL_STYLE = -16;
        public const int WS_MAXIMIZEBOX = 0x00010000;
        public const int WS_MINIMIZEBOX = 0x00020000;
        public const int WS_CLIPCHILDREN = 0x02000000;
        public const int WS_CLIPSIBLINGS = 0x04000000;

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_CONTEXTHELP = 0x00000400;
        public const int WS_EX_LAYERED = 0x00080000;

        public const int SC_CONTEXTHELP = 0xf180;

        public const int ICON_SMALL = 0;
        public const int ICON_BIG = 1;

        public const int LWA_COLORKEY = 0x1;
        public const int LWA_ALPHA = 0x2;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 2;
        public const int HT_LEFT = 10;
        public const int HT_RIGHT = 11;
        public const int HT_TOP = 12;
        public const int HT_TOPLEFT = 13;
        public const int HT_TOPRIGHT = 14;
        public const int HT_BOTTOM = 15;
        public const int HT_BOTTOMLEFT = 16;
        public const int HT_BOTTOMRIGHT = 17;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBoxA(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        [DllImport("comdlg32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        [DllImport("Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetSaveFileName([In, Out] OpenFileName lpofn);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid,
            [MarshalAs(UnmanagedType.Interface)] out IShellItem ppv);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32", EntryPoint = "GetWindowLongA")]
        public static extern long GetWindowLong(IntPtr hwnd, long nIndex);

        [DllImport("user32", EntryPoint = "SetWindowLongA")]
        public static extern long SetWindowLong(IntPtr hwnd, long nIndex, long dwNewLong);

        [DllImport("user32")]
        public static extern long SetWindowPos(IntPtr hwnd, long hWndInsertAfter, long x, long y, long cx, long cy, long wFlags);

        [ComImport]
        [ClassInterface(ClassInterfaceType.None)]
        [TypeLibType(TypeLibTypeFlags.FCanCreate)]
        [Guid("DC1C5A9C-E88A-4DDE-A5A1-60F82A20AEF7")]
        public class FileOpenDialogRCW { }

        [ComImport()]
        [Guid("42F85136-DB7E-439C-85F1-E4075D135FC8")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IFileDialog
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            [PreserveSig()]
            public uint Show([In, Optional] IntPtr hwndOwner); //IModalWindow 

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetFileTypes([In] uint cFileTypes, [In, MarshalAs(UnmanagedType.LPArray)] IntPtr rgFilterSpec);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetFileTypeIndex([In] uint iFileType);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint GetFileTypeIndex(out uint piFileType);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint Advise([In, MarshalAs(UnmanagedType.Interface)] IntPtr pfde, out uint pdwCookie);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint Unadvise([In] uint dwCookie);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetOptions([In] uint fos);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint GetOptions(out uint fos);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, uint fdap);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint Close([MarshalAs(UnmanagedType.Error)] uint hr);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetClientGuid([In] ref Guid guid);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint ClearClientData();

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public uint SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
        }

        [ComImport]
        [Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellItem
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            uint BindToHandler([In] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, [Out, MarshalAs(UnmanagedType.Interface)] out IntPtr ppvOut);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            uint GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            uint GetDisplayName([In] uint sigdnName, out IntPtr ppszName);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            uint GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            uint Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);
        }
        
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class OpenFileName
        {
            public int structSize = 0;
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;
            public string filter;
            public string customFilter;
            public int maxCustFilter = 0;
            public int filterIndex = 0;
            public IntPtr file;
            public int maxFile = 0;
            public string fileTitle;
            public int maxFileTitle = 0;
            public string initialDir;
            public string title;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;
            public string defExt;
            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;
            public string templateName;
            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }

        [Flags]
        public enum OpenFileNameFlags
        {
            OFN_READONLY = 0x1,
            OFN_FORCESHOWHIDDEN = 0x10000000,
            OFN_OVERWRITEPROMPT = 0x2,
            OFN_HIDEREADONLY = 0x4,
            OFN_NOCHANGEDIR = 0x8,
            OFN_SHOWHELP = 0x10,
            OFN_ENABLEHOOK = 0x20,
            OFN_ENABLETEMPLATE = 0x40,
            OFN_ENABLETEMPLATEHANDLE = 0x80,
            OFN_NOVALIDATE = 0x100,
            OFN_ALLOWMULTISELECT = 0x200,
            OFN_EXTENSIONDIFFERENT = 0x400,
            OFN_PATHMUSTEXIST = 0x800,
            OFN_FILEMUSTEXIST = 0x1000,
            OFN_CREATEPROMPT = 0x2000,
            OFN_SHAREAWARE = 0x4000,
            OFN_NOREADONLYRETURN = 0x8000,
            OFN_NOTESTFILECREATE = 0x10000,
            OFN_NONETWORKBUTTON = 0x20000,
            /// <summary>
            /// Force no long names for 4.x modules
            /// </summary>
            OFN_NOLONGNAMES = 0x40000,
            /// <summary>
            /// New look commdlg
            /// </summary>
            OFN_EXPLORER = 0x80000,
            OFN_NODEREFERENCELINKS = 0x100000,
            /// <summary>
            /// Force long names for 3.x modules
            /// </summary>
            OFN_LONGNAMES = 0x200000
        }
    }
}
