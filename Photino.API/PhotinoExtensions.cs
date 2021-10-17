using System.Reflection;
using PhotinoNET;

namespace PhotinoAPI
{
    public static class PhotinoExtensions
    {
        public static PhotinoWindow RegisterApi(this PhotinoWindow window, PhotinoApi api)
        {
            api.Window = window;
            return window;
        }

        public static PhotinoWindow LoadResource(this PhotinoWindow window, string name)
        {
            window.LoadRawString(PhotinoUtil.GetResourceString(Assembly.GetCallingAssembly(), name));
            return window;
        }
    }
}
