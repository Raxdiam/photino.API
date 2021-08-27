using System;
using System.Runtime.InteropServices;
using static PhotinoAPI.Win32.NativeMethods;

namespace PhotinoAPI.Win32
{
    public class SaveFileDialog
    {
        private const int MAX_FILE_LENGTH = 4096;
        
        /// <summary>
        /// Gets or sets the folder in which the dialog will be opened.
        /// </summary>
        public string InitialDirectory { get; set; } = null;

        /// <summary>
        /// Gets or sets the selected filename.
        /// </summary>
        public string FileName { get; set; } = "\0";

        /// <summary>
        /// Gets of sets the filter.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the dialog title.
        /// </summary>
        public string Title { get; set; } = "Open";

        public bool ShowDialog(IntPtr? ownerHandle = null)
        {
            /*var ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            ofn.filter = Filter?.Replace("|", "\0") + "\0";
            ofn.file = FileName;
            ofn.nMaxFile = 512;
            ofn.title = Title;
            ofn.initialDir = InitialDirectory;
            ofn.dlgOwner = ownerHandle ?? IntPtr.Zero;
            var result = GetSaveFileName(ref ofn) ? DialogResult.OK : DialogResult.Cancel;
            FileName = ofn.lpstrFile;
            return result;*/

            var ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            ofn.filter = Filter?.Replace("|", "\0") + "\0";
            ofn.fileTitle = new string('\0', 256);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            ofn.initialDir = InitialDirectory;
            ofn.title = Title;
            ofn.flags = (int)(OpenFileNameFlags.OFN_HIDEREADONLY | OpenFileNameFlags.OFN_EXPLORER | OpenFileNameFlags.OFN_FILEMUSTEXIST | OpenFileNameFlags.OFN_PATHMUSTEXIST);
            ofn.file = Marshal.AllocHGlobal(MAX_FILE_LENGTH * Marshal.SystemDefaultCharSize);
            ofn.maxFile = MAX_FILE_LENGTH;
            ofn.dlgOwner = ownerHandle ?? IntPtr.Zero;

            var success = GetSaveFileName(ofn);

            FileName = Marshal.PtrToStringAuto(ofn.file);
            Marshal.FreeHGlobal(ofn.file);

            return success;
        }
    }
}