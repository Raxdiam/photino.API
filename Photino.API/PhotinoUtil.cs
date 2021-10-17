using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using PhotinoAPI.Modules;
using PhotinoNET;

namespace PhotinoAPI
{
    internal static class PhotinoUtil
    {
        private const int WM_NCLBUTTONDOWN = 0xA1;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public static void HitTest(PhotinoWindow window, PhotinoHitTest hitTest)
        {
            ReleaseCapture();
            SendMessage(window.WindowHandle, WM_NCLBUTTONDOWN, (int)hitTest, 0);
        }

        public static string GetModuleName(Type type)
        {
            var attr = type.GetCustomAttribute<PhotinoNameAttribute>();
            return attr != null ? attr.Name : type.Name.ToCamelCase();
        }

        public static string GetMethodName(MethodInfo info)
        {
            var attr = info.GetCustomAttribute<PhotinoNameAttribute>();
            return attr != null ? attr.Name : info.Name.ToCamelCase();
        }
        
        public static string GetResourceString(Assembly assembly, string name)
        {
            var resourceNames = assembly.GetManifestResourceNames();
            var indexHtmlResource = resourceNames.FirstOrDefault((n) => n.EndsWith(name));
            if (indexHtmlResource == null) throw new Exception("Could not find 'index.html' in resources");

            using var indexHtmlStream = assembly.GetManifestResourceStream(indexHtmlResource);
            if (indexHtmlStream == null) throw new Exception("Could not get mainfest resource stream for resource 'index.html'");

            using var indexHtmlReader = new StreamReader(indexHtmlStream);

            return indexHtmlReader.ReadToEnd();
        }

        public static string Shell => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "/bin/bash";

        public static string Execute(string command)
        {
            var format = OS == OSPlatform.Windows ? "/c {0}" : "-c \"{0}\"";
            var cmd = OS == OSPlatform.Windows ? command : command.Replace("\"", "\\\"");
            var psi = new ProcessStartInfo
            {
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

        public static OSPlatform OS
        {
            get
            {
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

    internal enum PhotinoHitTest
    {
        Caption = 2,
        Left = 10,
        Right = 11,
        Top = 12,
        TopLeft = 13,
        TopRight = 14,
        Bottom = 15,
        BottomLeft = 16,
        BottomRight = 17
    }
}
