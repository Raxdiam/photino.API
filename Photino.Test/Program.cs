using PhotinoAPI;
using PhotinoNET;

namespace PhotinoTest;

internal class Program
{
    private static void Main()
    {
        var window = new PhotinoWindow()
            .SetTitle("Test Window")
            .SetUseApi()
            .Center()
            .Load("http://localhost:3000");

        window.WaitForClose();
    }
}
