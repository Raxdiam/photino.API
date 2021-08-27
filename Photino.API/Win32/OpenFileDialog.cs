using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using static PhotinoAPI.Win32.NativeMethods;

namespace PhotinoAPI.Win32
{
    public class OpenFileDialog
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
        /// Gets or sets the selected filenames.
        /// </summary>
        public string[] FileNames { get; set; }

        /// <summary>
        /// Gets of sets the filter.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Gets or sets the dialog title.
        /// </summary>
        public string Title { get; set; } = "Open";
        
        /// <summary>
        /// Gets or sets multiple file selection.
        /// </summary>
        public bool Multiselect { get; set; }

        /// <summary>
        /// Gets or sets show hidden files.
        /// </summary>
        public bool ShowHidden { get; set; }

        public bool ShowDialog(IntPtr? ownerHandle = null)
        {
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

            for (var i = 0; i < MAX_FILE_LENGTH * Marshal.SystemDefaultCharSize; i++) {
                Marshal.WriteByte(ofn.file, i, 0);
            }

            if (ShowHidden) ofn.flags |= (int)OpenFileNameFlags.OFN_FORCESHOWHIDDEN;
            if (Multiselect) ofn.flags |= (int)OpenFileNameFlags.OFN_ALLOWMULTISELECT;

            var success = GetOpenFileName(ofn);

            if (success) {
                var filePtr = ofn.file;
                var pointer = (long)filePtr;
                var file = Marshal.PtrToStringAuto(filePtr);
                var files = new List<string>();

                while (file?.Length > 0) {
                    files.Add(file);
                    pointer += file.Length * Marshal.SystemDefaultCharSize + Marshal.SystemDefaultCharSize;
                    filePtr = (IntPtr)pointer;
                    file = Marshal.PtrToStringAuto(filePtr);
                }

                if (files.Count > 1) {
                    FileNames = new string[files.Count - 1];
                    for (var i = 1; i < files.Count; i++) {
                        FileNames[i - 1] = Path.Combine(files[0], files[i]);
                    }
                }
                else {
                    FileNames = files.ToArray();
                }

                FileName = FileNames.FirstOrDefault();
            }

            Marshal.FreeHGlobal(ofn.file);

            return success;
        }
    }
}