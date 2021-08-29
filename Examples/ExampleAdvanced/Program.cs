using System;
using PhotinoAPI;
using PhotinoNET;

namespace ExampleAdvanced
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var manager = new PhotonManager { HandleHitTest = true };
            var window = new PhotinoWindow()
                .SetTitle("Advanced Example")
                .SetUseOsDefaultSize(false)
                .SetSize(800, 700)
                .SetChromeless(true)
                .SetLogVerbosity(0)
                .Center()
                .RegisterPhotonManager(manager)
                //.LoadResource("index.html");
                .Load("http://localhost:3000");

            window.WaitForClose();
        }
    }
}
