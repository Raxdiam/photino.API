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
            var manager = new PhotonManager();
            var window = new PhotinoWindow()
                .SetTitle("Test App")
                .SetUseOsDefaultSize(false)
                .SetSize(800, 700)
                .SetContextMenuEnabled(false)
                .SetDevToolsEnabled(true)
                .RegisterPhotonManager(manager)
                .LoadRawString(GetIndexFromResources());
                //.Load("http://localhost:3000");

            window.WaitForClose();
        }

        private static string GetIndexFromResources()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames();
            var indexHtmlResource = resourceNames.FirstOrDefault((n) => n.EndsWith("index.html"));
            if (indexHtmlResource == null) throw new Exception("Could not find 'index.html' in resources");

            using var indexHtmlStream = assembly.GetManifestResourceStream(indexHtmlResource);
            if (indexHtmlStream == null) throw new Exception("Could not get mainfest resource stream for resource 'index.html'");

            using var indexHtmlReader = new StreamReader(indexHtmlStream);
            
            return indexHtmlReader.ReadToEnd();
        }
    }
}
