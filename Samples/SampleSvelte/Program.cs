using System;
using PhotinoAPI;
using PhotinoNET;

namespace SampleSvelte
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var api = new PhotinoApi();
            var window = new PhotinoWindow()
                .SetTitle("Svelte Sample")
                .SetSize(700, 600)
                .SetUseOsDefaultSize(false)
                .Center()
                .RegisterApi(api)
                .Load("http://localhost:3000");
            window.WaitForClose();
        }
    }
}
