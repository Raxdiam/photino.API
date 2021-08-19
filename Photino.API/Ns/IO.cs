using System;
using System.IO;
using System.Text;

namespace PhotinoAPI.Ns
{
    [PhotonName("io")]
    public class IO : PhotonApiBase
    {
        public IO(PhotonManager manager) : base(manager) { }
        
        [PhotonName("readFile")]
        public static string ReadFile(string path, string encoding = null) =>
            encoding == null ? File.ReadAllText(path) : File.ReadAllText(path, Encoding.GetEncoding(encoding));

        [PhotonName("listFiles")]
        public static string[] ListFiles(string path, string searchPattern = null, bool recursive = false)
        {
            if (searchPattern != null && recursive)
                return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
            if (searchPattern != null)
                return Directory.GetFiles(path, searchPattern);
            return recursive ? Directory.GetFiles(path, "*", SearchOption.AllDirectories) : Directory.GetFiles(path);
        }

        [PhotonName("listFolders")]
        public static string[] ListFolders(string path, string searchPattern = null, bool recursive = false)
        {
            if (searchPattern != null && recursive)
                return Directory.GetDirectories(path, searchPattern, SearchOption.AllDirectories);
            if (searchPattern != null)
                return Directory.GetDirectories(path, searchPattern);
            return recursive ? Directory.GetDirectories(path, "*", SearchOption.AllDirectories) : Directory.GetDirectories(path);
        }

        [PhotonName("writeFile")]
        public static void WriteFile(string path, string contents, string encoding = null)
        {
            if (encoding == null)
                File.WriteAllText(path, contents);
            else
                File.WriteAllText(path, contents, Encoding.GetEncoding(encoding));
        }

        [PhotonName("createFolder")]
        public static void CreateFolder(string path) => Directory.CreateDirectory(path);

        [PhotonName("deleteFile")]
        public static void DeleteFile(string path) => File.Delete(path);

        [PhotonName("deleteFolder")]
        public static void DeleteFolder(string path, bool recursive = false) => Directory.Delete(path, recursive);
        
        [PhotonName("fileExists")]
        public static bool FileExists(string path) => File.Exists(path);

        [PhotonName("cwd")]
        public static string CurrentFolder() => Environment.CurrentDirectory;
    }
}