using System;
using System.IO;
using System.Linq;
using PhotinoAPI.Platform;
using PhotinoAPI.Win32;

namespace PhotinoAPI.Ns
{
    [PhotonName("os")]
    public class OS : PhotonApiBase
    {
        public OS(PhotonManager manager) : base(manager) { }

        [PhotonName("isWindows")]
        public static bool IsWindows() => PhotonManager.IsWindows;

        [PhotonName("isLinux")]
        public static bool IsLinux() => PhotonManager.IsLinux;

        [PhotonName("isFreeBSD")]
        public static bool IsFreeBSD() => PhotonManager.IsFreeBSD;

        [PhotonName("isOSX")]
        public static bool IsOSX() => PhotonManager.IsOSX;

        [PhotonName("joinPaths")]
        public static string JoinPaths(string[] paths) => Path.Join(paths);

        [PhotonName("getEnvar")]
        public static string GetEnvar(string key) => Environment.GetEnvironmentVariable(key);

        [PhotonName("cmd")]
        public static string Cmd(string command)
        {
            return Term.Execute(command);
        }

        [PhotonName("showOpenFileDialog")]
        public DialogResult ShowOpenFileDialog(string title, bool multiselect, DialogFilter[] filters)
        {
            var safeTitle = title ?? "Open";

            if (IsWindows()) {
                var ofd = new OpenFileDialog {
                    Title = safeTitle,
                    Filter = filters?.Select(f => $"{f.Name}|{f.Patterns.Join(";")}").Join("|"),
                    Multiselect = multiselect
                };
                var success = ofd.ShowDialog(Window.WindowHandle);
                return new(success, success ? ofd.FileName : null, success ? ofd.FileNames : null);
            }

            if (IsLinux() || IsFreeBSD()) {
                var filter = filters?.Select(f => $"--file-filter='{f.Name}|{f.Patterns.Join(" ")}'").Join(" ");
                var command = $"zenity --file-selection --title='{safeTitle}'{(filters is { Length: > 0 } ? " " + filter : "")}";
                var output = Term.Execute(command);
                var success = !output.IsVoid();
                return new(success, success ? output : null); //TODO: multiselect output
            }

            if (IsOSX()) {
                DialogFilter bad;
                if ((bad = filters.FirstOrDefault(f => f.Patterns.Contains("*.*"))) != null) {
                    var tempList = filters.ToList();
                    tempList.Remove(bad);
                    filters = tempList.ToArray();
                }

                var filter = filters is { Length: > 0 } ? "{ " + filters.SelectMany(f => f.Patterns).Select(f => $"\"{f.Replace("*.", "")}\"").Join(", ") + " }" : null;
                var command = $"osascript -e 'POSIX path of (choose file with prompt \"{safeTitle}\"{(filter != null ? $" of type {filter}" : "")})'";
                var output = Term.Execute(command);
                var success = !output.IsVoid();
                return new(success, success ? output : null); //TODO: multiselect output
            }

            return new();
        }

        [PhotonName("showOpenFolderDialog")]
        public DialogResult ShowOpenFolderDialog(string title)
        {
            var safeTitle = title ?? "Open";

            if (IsWindows()) {
                var ofd = new OpenFolderDialog { Title = safeTitle };
                var success = ofd.ShowDialog(Window.WindowHandle);
                return new(success, success ? ofd.Folder : null);
            }

            if (IsLinux() || IsFreeBSD()) {
                var command = $"zenity --file-selection --title='{safeTitle}' --directory";
                var output = Cmd(command);
                return new() { Success = !output.IsVoid(), Path = output };
            }

            if (IsOSX()) {
                var command = $"osascript -e 'POSIX path of (choose folder with prompt \"{safeTitle}\")";
                var output = Cmd(command);
                return new() { Success = !output.IsVoid(), Path = output };
            }

            return new();
        }

        [PhotonName("showSaveFileDialog")]
        public DialogResult ShowSaveFileDialog(string title, DialogFilter[] filters)
        {
            var safeTitle = title ?? "Save";

            if (IsWindows()) {
                var sfd = new SaveFileDialog {
                    Title = safeTitle,
                    Filter = filters?.Select(f => $"{f.Name}|{f.Patterns.Join(";")}").Join("|")
                };
                var success = sfd.ShowDialog(Window.WindowHandle);
                return new(success, success ? sfd.FileName : null, success ? new[] { sfd.FileName } : null);
            }

            if (IsLinux() || IsFreeBSD()) {
                var filter = filters?.Select(f => $"--file-filter='{f.Name}|{f.Patterns.Join(" ")}'").Join(" ");
                var command = $"zenity --file-selection --save --title='{safeTitle}'{(filters is { Length: > 0 } ? " " + filter : "")}";
                var output = Cmd(command);
                var success = !output.IsVoid();
                return new(success, success ? output : null, success ? new[] { output } : null);
            }

            if (IsOSX()) {
                DialogFilter bad;
                if ((bad = filters.FirstOrDefault(f => f.Patterns.Contains("*.*"))) != null) {
                    var tempList = filters.ToList();
                    tempList.Remove(bad);
                    filters = tempList.ToArray();
                }

                var filter = filters is { Length: > 0 } ? "{ " + filters.SelectMany(f => f.Patterns).Select(f => $"\"{f.Replace("*.", "")}\"").Join(", ") + " }" : null;
                var command = $"osascript -e 'POSIX path of (choose file name with prompt \"{safeTitle}\"{(filter != null ? $" of type {filter}" : "")})";
                var output = Cmd(command);
                return new() { Success = !output.IsVoid(), Path = output, Paths = new[] { output } };
            }

            return new();
        }

        //TODO: messagebox
    }

    public record FileDialogOptions(DialogFilter[] Filters);
    public record DialogFilter(string Name, string[] Patterns);
    public record DialogResult(bool Success = false, string Path = null, string[] Paths = null);
    public record MessageBoxResult(bool YesOrOk = false, bool HasError = false, string Error = null);

    public enum MessageBoxType
    {
        Info,
        Warning,
        Error,
        Question
    }
}