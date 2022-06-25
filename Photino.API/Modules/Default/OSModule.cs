using System;
using PhotinoNET;

namespace PhotinoAPI.Modules.Default
{
    internal class OSModule : PhotinoModuleBase
    {
        public OSModule(PhotinoApi api) : base(api) { }

        public static bool IsWindows() => PhotinoWindow.IsWindowsPlatform;
        public static bool IsLinux() => PhotinoWindow.IsLinuxPlatform;
        public static bool IsMacOs() => PhotinoWindow.IsMacOsPlatform;
        public static string GetEnvar(string key) => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
        public static string Cmd(string command) => PhotinoUtil.Execute(command);

        public string[] ShowOpenFile(string title, string[] filters, bool multiSelect, string defaultPath) =>
            Api.Window.ShowOpenFile(title, filters, multiSelect ? DialogOption.MultiSelect : DialogOption.None, defaultPath);
        public string ShowSaveFile(string title, string[] filters, string defaultPath) => Api.Window.ShowSaveFile(title, filters, defaultPath);
        public string ShowSelectFolder(string title, string defaultPath) => Api.Window.ShowSelectFolder(title, defaultPath);
    }
}
