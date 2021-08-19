using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PhotinoAPI.Platform
{
    internal static class Term
    {
        public static string Shell => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";
        
        public static string Execute(string command)
        {
            var format = OS == OSPlatform.Windows ? "/c {0}" : "-c \"{0}\"";
            var cmd = OS == OSPlatform.Windows ? command : command.Replace("\"", "\\\"");
            var psi = new ProcessStartInfo {
                FileName = Shell,
                Arguments = string.Format(format, cmd),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            
            var proc = new Process { StartInfo = psi };
            proc.Start();
            
            proc.WaitForExit();
            return proc.StandardOutput.ReadToEnd();
        }

        private static OSPlatform OS {
            get {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return OSPlatform.Windows;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    return OSPlatform.Linux;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                    return OSPlatform.FreeBSD;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    return OSPlatform.OSX;
                return OSPlatform.Create("Unknown");
            }
        }
    }
}