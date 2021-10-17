using System;

namespace PhotinoAPI.Modules.Default
{
    [PhotinoName("app")]
    internal class AppModule : PhotinoModuleBase
    {
        public static void Exit(int exitCode = 0) => Environment.Exit(exitCode);

        public static string Cwd() => Environment.CurrentDirectory;
    }
}
