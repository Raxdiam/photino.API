using System;
using PhotinoNET;

namespace PhotinoAPI
{
    public class PhotinoApi
    {
        internal const string ApiPrefix = "papi";
        internal const string EventPrefix = "pev";

        internal static PhotinoApi Singleton;
        internal static PhotinoWindow MainWindow;
        internal static PhotinoManager Manager;

        internal static void Init(PhotinoWindow mainWindow)
        {
            Singleton = new PhotinoApi(); // probably don't even need an instance. may make this class static
            MainWindow = mainWindow;
            Manager = new PhotinoManager(mainWindow); // is there even a point in having all that stuff in a separate class?
        }

        internal static void Send<T>(string prefix, string name, T data, string id = null, PhotinoWindow window = null, bool throwError = false)
        {
            window ??= MainWindow;
            if (window == null) return;

            var msg = new PhotinoMessage { Id = id, Name = name, Data = data };

            string json;
            try {
                json = msg.ToJson();
            }
            catch {
                if (throwError)
                    throw;
                json = null;
            }
            if (json == null) return;

            window.SendWebMessage($"{ApiPrefix}:{json}");
        }

        internal static void Callback<T>(ref PhotinoMessage msg, T data, PhotinoWindow window = null) => Send(ApiPrefix, msg.Name, data, msg.Id, window);

        internal static void Event(string name, object data, PhotinoWindow window = null) => Send(EventPrefix, name, data, window: window);

        internal static void Error(Exception ex, PhotinoWindow window = null) => Send(ApiPrefix, "error", new { source = ex.Source, stacktrace = ex.StackTrace, message = ex.Message }, window: window);

        internal static void Error(string message, PhotinoWindow window = null) => Error(new Exception(message), window);
    }
}
