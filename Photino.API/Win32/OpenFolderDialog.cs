using System;
using System.Runtime.InteropServices;
using static PhotinoAPI.Win32.NativeMethods;

namespace PhotinoAPI.Win32
{
    /// <summary>
    /// Prompts the user to select a folder.
    /// </summary>
    public class OpenFolderDialog
    {
        /// <summary>
        /// Gets or sets the folder in which the dialog will be opened.
        /// </summary>
        public string InitialFolder { get; set; }

        /// <summary>
        /// Gets or sets the directory in which the dialog will be opened if there is no recent directory available.
        /// </summary>
        public string DefaultFolder { get; set; }

        /// <summary>
        /// Gets or sets the selected folder.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets the dialog title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Displays the dialog with the specified owner.
        /// </summary>
        /// <param name="ownerHandle">Any object that implements IWin32Window that represents the top-level window that will own the modal dialog box. Can be NULL for specifying no owner.</param>
        /// <returns></returns>
        public bool ShowDialog(IntPtr? ownerHandle = null)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var fd = (IFileDialog)new FileOpenDialogRCW();
            fd.GetOptions(out var options);
            options |= FOS_PICKFOLDERS | FOS_FORCEFILESYSTEM | FOS_NOVALIDATE | FOS_NOTESTFILECREATE | FOS_DONTADDTORECENT;
            fd.SetOptions(options);
            if (InitialFolder != null) {
                var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); //IShellItem
                if (SHCreateItemFromParsingName(InitialFolder, IntPtr.Zero, ref riid, out var directoryShellItem) == S_OK) {
                    fd.SetFolder(directoryShellItem);
                }
            }

            if (DefaultFolder != null) {
                var riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"); //IShellItem
                if (SHCreateItemFromParsingName(DefaultFolder, IntPtr.Zero, ref riid, out var directoryShellItem) == S_OK) {
                    fd.SetDefaultFolder(directoryShellItem);
                }
            }

            if (fd.Show(ownerHandle ?? IntPtr.Zero) == S_OK) {
                if (fd.GetResult(out var shellItem) == S_OK) {
                    if (shellItem.GetDisplayName(SIGDN_FILESYSPATH, out var pszString) == S_OK) {
                        if (pszString != IntPtr.Zero) {
                            try {
                                Folder = Marshal.PtrToStringAuto(pszString);
                                return true;
                            }
                            finally {
                                Marshal.FreeCoTaskMem(pszString);
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}