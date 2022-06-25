using System.Net.Http;
using Newtonsoft.Json;
using PhotinoNET;

namespace PhotinoAPI
{
    internal class PhotinoApiCallbacks
    {
        private readonly PhotinoWindow _window;

        public PhotinoApiCallbacks(PhotinoWindow window)
        {
            _window = window;
            _window.WebMessageReceived += OnApiEventRequestWebMessageReceived;
        }

        private void OnApiEventRequestWebMessageReceived(object sender, string e)
        {
            if (!e.StartsWith("cb:")) return;

            var json = JsonConvert.DeserializeObject<DownloadFileData>(e[3..], PhotinoUtil.JsonSettings);
        }

        public class CallbackRequest
        {
            public string Name { get; set; }
            public string Id { get; set; }

        }

        public class DownloadFileData
        {
            public string Url { get; set; }
            public string Destination { get; set; }
        }
    }
}
