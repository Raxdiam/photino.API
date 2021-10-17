using System;
using PhotinoNET;

namespace PhotinoAPI.Modules.Default
{
    internal class OSModule : PhotinoModuleBase
    {
        public static bool IsWindows() => PhotinoWindow.IsWindowsPlatform;
        public static bool IsLinux() => PhotinoWindow.IsLinuxPlatform;
        public static bool IsMacOs() => PhotinoWindow.IsMacOsPlatform;
        public static string GetEnvar(string key) => Environment.GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
        public static string Cmd(string command) => PhotinoUtil.Execute(command);
    }
}
