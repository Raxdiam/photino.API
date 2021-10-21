using System;
using PhotinoAPI;
using PhotinoNET;

namespace Photino.Test
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var api = new PhotinoApi();
            var window = new PhotinoWindow()
                .SetTitle("API Test App")
                .SetUseOsDefaultSize(false)
                .SetSize(700, 600)
                .Center()
                .RegisterApi(api)
#if DEBUG
                .Load("http://localhost:5500");
#else
                .Load("wwwroot/index.html");
#endif
            window.WaitForClose();
        }
    }
}
