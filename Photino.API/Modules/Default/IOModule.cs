using System;
using System.IO;
using System.Linq;
using System.Text;

namespace PhotinoAPI.Modules.Default
{
    [PhotinoName("io")]
    internal class IOModule : PhotinoModuleBase
    {
        [PhotinoName("readFile")]
        public static byte[] ReadFile(string path) => File.ReadAllBytes(path);

        public static string ReadFileText(string path, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                return File.ReadAllText(path);
            return File.ReadAllText(path, enc);
        }

        public static string[] ReadFileLines(string path, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                return File.ReadAllLines(path);
            return File.ReadAllLines(path, enc);
        }
        
        public static void WriteFile(string path, byte[] data) => File.WriteAllBytes(path, data);

        public static void WriteFileText(string path, string contents, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                File.WriteAllText(path, contents);
            else
                File.WriteAllText(path, contents, enc);
        }

        public static void WriteFileLines(string path, string[] contents, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                File.WriteAllLines(path, contents);
            else
                File.WriteAllLines(path, contents, enc);
        }

        public static string[] ListFiles(string path, string searchPattern = null, bool recursive = false)
        {
            if (searchPattern != null && recursive) return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
            if (searchPattern != null) return Directory.GetFiles(path, searchPattern);
            if (recursive) return Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            return Directory.GetFiles(path);
        }

        public static string[] ListFolders(string path, string searchPattern = null, bool recursive = false)
        {
            if (searchPattern != null && recursive) return Directory.GetDirectories(path, searchPattern, SearchOption.AllDirectories);
            if (searchPattern != null) return Directory.GetDirectories(path, searchPattern);
            if (recursive) return Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            return Directory.GetDirectories(path);
        }

        public static void CreateFolder(string path) => Directory.CreateDirectory(path);

        public static void DeleteFile(string path) => File.Delete(path);

        public static void DeleteFolder(string path, bool recursive = false) => Directory.Delete(path, recursive);

        public static bool FileExists(string path) => File.Exists(path);

        public static bool FolderExists(string path) => Directory.Exists(path);
        
        public static string ResolvePath(string path) => Path.GetFullPath(path);

        public static string GetExtension(string path) => Path.GetExtension(path);
        
        private static bool TryGetEncoding(string value, out Encoding encoding)
        {
            var all = Encoding.GetEncodings().Select(e => e.Name).ToArray();
            if (all.Contains(value, StringComparer.CurrentCultureIgnoreCase)) {
                encoding = Encoding.GetEncoding(value);
                return true;
            }

            encoding = null;
            return false;
        }
    }
}
