using System;
using System.Drawing;
using PhotinoNET;

namespace PhotinoAPI
{
    /// <summary>
    /// Event manager for Photino.API
    /// </summary>
    internal class PhotinoApiEvents
    {
        private readonly PhotinoWindow _window;

        public PhotinoApiEvents(PhotinoWindow window)
        {
            _window = window;
            _window.WebMessageReceived += OnApiEventRequestWebMessageReceived;
        }

        public void Reset()
        {
            _window.WindowSizeChanged -= OnWindowSizeChanged;
            _window.WindowLocationChanged -= OnWindowLocationChanged;
            _window.WindowClosing -= OnWindowClosing;
        }

        public void Clear()
        {
            _window.WebMessageReceived -= OnApiEventRequestWebMessageReceived;
            Reset();
        }
        
        private void OnApiEventRequestWebMessageReceived(object sender, string e)
        {
            if (!e.StartsWith("ev:")) return;

            var type = e.Split(':')[1];
            switch (type) {
                case "reset":
                    Reset();
                    break;
                case "size":
                    _window.WindowSizeChanged -= OnWindowSizeChanged;
                    _window.WindowSizeChanged += OnWindowSizeChanged;
                    break;
                case "location":
                    _window.WindowLocationChanged -= OnWindowLocationChanged;
                    _window.WindowLocationChanged += OnWindowLocationChanged;
                    break;
                case "closing":
                    _window.WindowClosing -= OnWindowClosing;
                    _window.WindowClosing += OnWindowClosing;
                    break;
            }
        }
        
        private void OnWindowSizeChanged(object sender, Size e)
        {
            _window.SendWebMessage($"ev:size|width={e.Width},height={e.Height}");
        }
        
        private void OnWindowLocationChanged(object sender, Point e)
        {
            _window.SendWebMessage($"ev:location|x={e.X},y={e.Y}");
        }
        
        private bool OnWindowClosing(object sender, EventArgs e)
        {
            _window.SendWebMessage("ev:closing");
            return false;
        }
    }
}