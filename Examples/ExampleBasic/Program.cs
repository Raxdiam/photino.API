using System;
using System.IO;
using System.Linq;
using System.Reflection;
using PhotinoAPI;
using PhotinoNET;

namespace TestApp
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
                .SetTitle("Basic Example")
                .SetUseOsDefaultSize(false)
                .SetSize(800, 700)
                .Center()
                .RegisterPhotonManager(manager)
                .LoadResource("index.html");

            window.WaitForClose();
        }
    }
}
