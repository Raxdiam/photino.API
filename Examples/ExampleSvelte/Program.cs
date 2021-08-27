using System;
using PhotinoAPI;
using PhotinoNET;

namespace ExampleSvelte
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var manager = new PhotonManager {
                HandleHitTest = true
            };
            var window = new PhotinoWindow()
                .SetTitle("Svelte Example")
                .SetUseOsDefaultSize(false)
                .SetSize(800, 700)
                .Center()
                .RegisterPhotonManager(manager)
                .LoadResource("index.html");

            window.WaitForClose();
        }
    }
}
