using PhotinoAPI;
using PhotinoNET;
using System;

namespace SampleBasic
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var api = new PhotinoApi();
            var window = new PhotinoWindow()
                .SetTitle("Basic Sample")
                .SetUseOsDefaultSize(false)
                .SetSize(700, 600)
                .Center()
                .RegisterApi(api)
                .Load("wwwroot/index.html");

            window.WaitForClose();
        }
    }
}
