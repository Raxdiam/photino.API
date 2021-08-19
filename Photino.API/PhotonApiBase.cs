using System.Runtime.InteropServices;
using PhotinoNET;

namespace PhotinoAPI
{
    public abstract class PhotonApiBase
    {
        protected PhotonApiBase(PhotonManager manager)
        {
            Manager = manager;
        }
        
        protected PhotonManager Manager { get; }

        protected PhotinoWindow Window => Manager.Window;

        protected static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        protected static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        protected static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
        protected static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
}