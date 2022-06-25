using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotinoAPI.Modules.Default
{
    [PhotinoName("io")]
    internal class IOModule : PhotinoModuleBase
    {
        public static async Task<byte[]> ReadFile(string path) => await File.ReadAllBytesAsync(path);

        public static async Task<string> ReadFileText(string path, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                return await File.ReadAllTextAsync(path);
            return await File.ReadAllTextAsync(path, enc);
        }

        public static async Task<string[]> ReadFileLines(string path, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                return await File.ReadAllLinesAsync(path);
            return await File.ReadAllLinesAsync(path, enc);
        }
        
        public static async Task WriteFile(string path, byte[] data) => await File.WriteAllBytesAsync(path, data);

        public static async Task WriteFileText(string path, string contents, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                await File.WriteAllTextAsync(path, contents);
            else
                await File.WriteAllTextAsync(path, contents, enc);
        }

        public static async Task WriteFileLines(string path, string[] contents, string encoding = null)
        {
            if (encoding == null || !TryGetEncoding(encoding, out var enc))
                await File.WriteAllLinesAsync(path, contents);
            else
                await File.WriteAllLinesAsync(path, contents, enc);
        }

        public static void MoveFile(string path, string destination) => File.Move(path, destination);

        public static void MoveFolder(string path, string destination) => Directory.Move(path, destination);

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
