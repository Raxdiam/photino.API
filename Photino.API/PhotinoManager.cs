using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhotinoNET;

namespace PhotinoAPI
{
    internal class PhotinoManager
    {
        private readonly PhotinoWindow _window;
        private readonly Dictionary<string, Func<object, object>> _handlers;
        private readonly Dictionary<string, RegistryItem> _eventRegistry;

        private bool _winodwCreated;

        private class RegistryItem
        {
            public RegistryItem(bool register, Delegate del, Delegate handler) => (Register, Del, Handler) = (register, del, handler);

            public bool Register;
            public readonly Delegate Del;
            public readonly Delegate Handler;
        }

        public PhotinoManager(PhotinoWindow window)
        {
            _window = window;
            _handlers = new();

            _window.WebMessageReceived += OnWebMessageReceived;
            _window.WindowCreated += OnWindowCreated;

            _eventRegistry = new() {
                ["focus_in"] = new(false, window.WindowFocusInHandler, new EventHandler((_, _) => PhotinoApi.Event("focus_in", null))),
                ["focus_out"] = new(false, window.WindowFocusOutHandler, new EventHandler((_, _) => PhotinoApi.Event("focus_put", null))),
                ["resized"] = new(false, window.WindowSizeChangedHandler, new EventHandler<Size>((_, e) => PhotinoApi.Event("resized", e))),
                ["moved"] = new(false, window.WindowLocationChangedHandler, new EventHandler<Point>((_, e) => PhotinoApi.Event("moved", e))),
                ["maximized"] = new(false, window.WindowMaximizedHandler, new EventHandler((_, _) => PhotinoApi.Event("maximized", null))),
                ["minimized"] = new(false, window.WindowMinimizedHandler, new EventHandler((_, _) => PhotinoApi.Event("minimized", null))),
                ["restored"] = new(false, window.WindowRestoredHandler, new EventHandler((_, _) => PhotinoApi.Event("restored", null)))
            };
        }
        
        public PhotinoManager AddHandler<TIn, TOut>(string name, Func<TIn, TOut> handler) 
            where TIn : class
            where TOut : class
        {
            _handlers.Add(name, (Func<object, object>)handler);
            return this;
        }
        
        private void HandleApiMessage(string e)
        {
            var json = e.Remove(0, PhotinoApi.ApiPrefix.Length + 1);
            if (!TryGetMessage(json, out var msg)) return;

            if (!_handlers.ContainsKey(msg.Name)) {
                PhotinoApi.Error($"No handler exists for '{msg.Name}'.");
                return;
            }

            var result = _handlers[msg.Name](msg.Data);
            if (result != null) PhotinoApi.Callback(ref msg, result, _window);
        }
        
        private void HandleEventMessage(string e)
        {
            var json = e.Remove(0, PhotinoApi.EventPrefix.Length + 1);
            if (!TryGetMessage(json, out var msg)) return;
            
            if (msg.Name is not "register" or "unregister") {
                PhotinoApi.Error("Event message name must be either 'register' or 'unregister'.");
                return;
            }

            string data;
            try {
                data = ((JToken)msg.Data).Value<string>();
            }
            catch (Exception ex) {
                PhotinoApi.Error(ex);
                return;
            }

            if (string.IsNullOrEmpty(data) || string.IsNullOrWhiteSpace(data)) {
                PhotinoApi.Error("Event name cannot be null or empty.");
                return;
            }

            if (!_eventRegistry.ContainsKey(data)) {
                PhotinoApi.Error($"The event '{data}' does not exist.");
                return;
            }

            if (msg.Name is "register")
                _eventRegistry[data].Register = true;
            else if (msg.Name is "unregister")
                _eventRegistry[data].Register = false;
            
            UpdateEventHandlers();
        }
        
        private void UpdateEventHandlers()
        {
            if (!_winodwCreated) return;
            foreach (var (name, e) in _eventRegistry) {
                var subbed = e.Del.GetInvocationList().Length > 0;
                switch (name) {
                    case "focus_in":
                        switch (e.Register) {
                            case true when !subbed:
                                _window.WindowFocusIn += (EventHandler)e.Handler;
                                break;
                            case false when subbed:
                                _window.WindowFocusIn -= (EventHandler)e.Handler;
                                break;
                        }
                        break;
                    case "focus_out":
                        switch (e.Register) {
                            case true when !subbed:
                                _window.WindowFocusOut += (EventHandler)e.Handler;
                                break;
                            case false when subbed:
                                _window.WindowFocusOut -= (EventHandler)e.Handler;
                                break;
                        }
                        break;
                    case "resized":
                        switch (e.Register) {
                            case true when !subbed:
                                _window.WindowSizeChanged += (EventHandler<Size>)e.Handler;
                                break;
                            case false when subbed:
                                _window.WindowSizeChanged -= (EventHandler<Size>)e.Handler;
                                break;
                        }
                        break;
                    case "moved":
                        switch (e.Register) {
                            case true when !subbed:
                                _window.WindowLocationChanged += (EventHandler<Point>)e.Handler;
                                break;
                            case false when subbed:
                                _window.WindowLocationChanged -= (EventHandler<Point>)e.Handler;
                                break;
                        }
                        break;
                    case "maximized":
                        switch (e.Register) {
                            case true when !subbed:
                                _window.WindowMaximized += (EventHandler)e.Handler;
                                break;
                            case false when subbed:
                                _window.WindowMaximized -= (EventHandler)e.Handler;
                                break;
                        }
                        break;
                    case "minimized":
                        switch (e.Register) {
                            case true when !subbed:
                                _window.WindowMinimized += (EventHandler)e.Handler;
                                break;
                            case false when subbed:
                                _window.WindowMinimized -= (EventHandler)e.Handler;
                                break;
                        }
                        break;
                    case "restored":
                        switch (e.Register) {
                            case true when !subbed:
                                _window.WindowRestored += (EventHandler)e.Handler;
                                break;
                            case false when subbed:
                                _window.WindowRestored -= (EventHandler)e.Handler;
                                break;
                        }
                        break;
                }
            }
        }

        private void OnWebMessageReceived(object sender, string e)
        {
            if (e.StartsWith($"{PhotinoApi.ApiPrefix}:")) {
                HandleApiMessage(e);
            }
            else if (e.StartsWith($"{PhotinoApi.EventPrefix}:")) {
                HandleEventMessage(e);
            }
        }
        
        private void OnWindowCreated(object sender, EventArgs e)
        {
            _winodwCreated = true;
            UpdateEventHandlers();
        }

        private static bool TryGetMessage(string json, out PhotinoMessage message)
        {
            message = null;

            if (string.IsNullOrEmpty(json) || string.IsNullOrWhiteSpace(json)) {
                PhotinoApi.Error("Message cannot be empty.");
                return false;
            }

            PhotinoMessage msg;
            try {
                msg = JsonConvert.DeserializeObject<PhotinoMessage>(json);
            }
            catch (Exception ex) {
                msg = null;
                PhotinoApi.Error(ex);
            }

            if (msg == null) {
                PhotinoApi.Error("Message cannot be null.");
                return false;
            }

            if (string.IsNullOrEmpty(msg.Name) || string.IsNullOrWhiteSpace(msg.Name)) {
                PhotinoApi.Error("Message name cannot be null or empty");
                return false;
            }

            message = msg;
            return true;
        }
    }
}
