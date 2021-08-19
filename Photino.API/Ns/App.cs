using System;

namespace PhotinoAPI.Ns
{
    [PhotonName("app")]
    public class App : PhotonApiBase
    {
        public App(PhotonManager manager) : base(manager) { }

        [PhotonName("exit")]
        public static void Exit(int exitCode = 0)
        {
            Environment.Exit(exitCode);
        }
    }
}
