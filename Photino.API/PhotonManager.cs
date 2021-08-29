using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using PhotinoAPI.Ns;
using PhotinoNET;
using static PhotinoAPI.Win32.NativeMethods;

namespace PhotinoAPI
{
    public class PhotonManager
    {
        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, IgnoreNullValues = true};
        private static readonly Dictionary<string, Dictionary<string, MethodInfo>> methodMap = new();
        private static readonly Dictionary<string, PhotonApiBase> instMap = new();

        private PhotinoWindow _window;
        private bool _handleHitTest;

        public PhotonManager()
        {
            Register<IO>();
            Register<OS>();
            Register<App>();
            Register<Window>();
        }

        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static bool IsFreeBSD => RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
        public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public PhotinoWindow Window {
            get => _window;
            private set => _window = value;
        }

        /// <summary>
        /// *** Windows Only ***
        /// <br/>Handles drag and resize hit tests via web message
        /// <br/>Web message hit test codes:
        /// <br/>- drag (Drag window)
        /// <br/>- rtl (Resize top-left)
        /// <br/>- rtr (Resize top-right)
        /// <br/>- rbl (Resize bottom-left)
        /// <br/>- rbr (Resize bottom-right)
        /// <br/>- rl (Resize left)
        /// <br/>- rt (Resize top)
        /// <br/>- rr (Resize right)
        /// <br/>- rb (Resize bottom)
        /// </summary>
        public bool HandleHitTest {
            get => _handleHitTest;
            set {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
                if (_window != null) {
                    _window.WebMessageReceivedHandler -= OnHitTestWebMessageReceived;
                    if (value) {
                        _window.WebMessageReceivedHandler += OnHitTestWebMessageReceived;
                    }
                }
                _handleHitTest = value;
            }
        }
        
        public PhotonManager Register<T>() where T : PhotonApiBase
        {
            var type = typeof(T);
            var inst = (T) Activator.CreateInstance(type, this);
            var nsNameAttr = type.GetCustomAttribute<PhotonNameAttribute>();
            if (nsNameAttr == null) {
                throw new Exception($"{type.Name} must have a {nameof(PhotonNameAttribute)}");
            }

            if (!methodMap.ContainsKey(nsNameAttr.Name)) {
                methodMap.Add(nsNameAttr.Name, new());
                instMap.Add(nsNameAttr.Name, inst);
            }
            
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            foreach (var method in methods) {
                var mNameAttr = method.GetCustomAttribute<PhotonNameAttribute>();
                if (mNameAttr == null) continue;

                if (!methodMap[nsNameAttr.Name].ContainsKey(mNameAttr.Name)) {
                    methodMap[nsNameAttr.Name].Add(mNameAttr.Name, method);
                }
            }
            
            return this;
        }

        private void OnHitTestWebMessageReceived(object sender, string message)
        {
            switch (message) {
                case "drag":
                    HitTest(_window, HT_CAPTION);
                    break;
                case "rtl": //Top Left
                    HitTest(_window, HT_TOPLEFT);
                    break;
                case "rtr": // Top Right
                    HitTest(_window, HT_TOPRIGHT);
                    break;
                case "rbl": // Bottom Left
                    HitTest(_window, HT_BOTTOMLEFT);
                    break;
                case "rbr": //Bottom Right
                    HitTest(_window, HT_BOTTOMRIGHT);
                    break;
                case "rl": // Left
                    HitTest(_window, HT_LEFT);
                    break;
                case "rt": // Top
                    HitTest(_window, HT_TOP);
                    break;
                case "rr": // Right
                    HitTest(_window, HT_RIGHT);
                    break;
                case "rb": // Bottom
                    HitTest(_window, HT_BOTTOM);
                    break;
            }
        }

        private void OnWebMessageReceived(object sender, string message)
        {
            if (!message.StartsWith("api{")) return;

            message = message.Remove(0, 3);

            var req = JsonSerializer.Deserialize<PhotonRequest>(message, jsonOptions);
            if (req == null) return;

            var res = new PhotonResponse { Id = req.Id, Data = new() };

            if (!methodMap.ContainsKey(req.Data.Ns)) {
                SendError(ref res, $"The API namespace '{req.Data.Ns}' does not exist");
                return;
            }

            if (!methodMap[req.Data.Ns].ContainsKey(req.Data.Action)) {
                SendError(ref res, $"The API action '{req.Data.Action}' does not exist in namespace {req.Data.Ns}");
                return;
            }

            var method = methodMap[req.Data.Ns][req.Data.Action];
            
            try {
                var reqParams = req.Data.Params;
                var mParams = method.GetParameters();

                foreach (var mParam in mParams) {
                    if (mParam.Name == null) {
                        SendError(ref res, @"A parameter's name on the backend is somehow null? ¯\_(ツ)_/¯ Guess I'll fail.");
                        return;
                    }

                    if (!reqParams.ContainsKey(mParam.Name) && !mParam.IsOptional) {
                        SendError(ref res, $"Required parameter '{mParam.Name}' was not included in the request for '{req.Data.Ns}.{req.Data.Action}'");
                        return;
                    }

                    if (!reqParams.ContainsKey(mParam.Name) && mParam.IsOptional) {
                        reqParams.Add(mParam.Name, null);
                        continue;
                    }

                    if (reqParams.ContainsKey(mParam.Name) && reqParams[mParam.Name] == null) {
                        continue;
                    }
                    
                    var jsonVal = (JsonElement)reqParams[mParam.Name];
                    var newVal = jsonVal.ToObject(mParam.ParameterType, jsonOptions);

                    if (newVal == null) {
                        SendError(ref res, $"Could not convert parameter '{mParam.Name}' to type '{mParam.ParameterType.Name}'");
                        return;
                    }

                    reqParams[mParam.Name] = newVal;
                }

                var parameters = req.Data.Params == null ? Array.Empty<object>() : req.Data.Params.Select(kv => kv.Value).ToArray();
                var ret = method.Invoke(method.IsStatic ? null : instMap[req.Data.Ns], parameters);
                SendSuccess(ref res, ret);
            }
            catch (Exception e) {
                SendError(ref res, e);
            }
        }

        private static void HitTest(PhotinoWindow window, int hitTest)
        {
            ReleaseCapture();
            SendMessage(window.WindowHandle, WM_NCLBUTTONDOWN, hitTest, 0);
        }

        private void SendError(ref PhotonResponse res, string message) => Send(ref res, PhotonStatus.Error, message: message);
        private void SendError(ref PhotonResponse res, Exception ex) => SendError(ref res, $"{ex.Message}\n{ex.StackTrace}");

        private void SendSuccess(ref PhotonResponse res, object ret) => Send(ref res, PhotonStatus.Success, ret);

        private void Send(ref PhotonResponse res, PhotonStatus status, object ret = null, string message = null)
        {
            res.Data.Status = status;
            res.Data.Return = ret;
            res.Data.Message = message;
            Window.SendWebMessage("api" + JsonSerializer.Serialize(res, jsonOptions));
        }

        private async void SetupEvents(object sender, EventArgs ea)
        {
            // These may be a bit much
            /*Window.RegisterSizeChangedHandler((_, e) => Window.SendWebMessage($"ev|size|width={e.Width}|height={e.Height}"));
            Window.RegisterLocationChangedHandler((_, e) => Window.SendWebMessage($"ev|location|x={e.X}|y={e.Y}"));*/

            await Task.Delay(1000); // An error is thrown about not being able to register events before the window is initiallized without this..
            Window.RegisterFocusInHandler((_, _) => Window.SendWebMessage("ev|focusIn"));
            Window.RegisterFocusOutHandler((_, _) => Window.SendWebMessage("ev|focusOut"));
            Window.RegisterMaximizedHandler((_, _) => Window.SendWebMessage("ev|maximized"));
            Window.RegisterRestoredHandler((_, _) => Window.SendWebMessage("ev|restored"));
            Window.RegisterMinimizedHandler((_, _) => Window.SendWebMessage("ev|minimized"));
        }
        
        internal void SetWindow(PhotinoWindow window)
        {
            Window = window;
            Window.RegisterWebMessageReceivedHandler(OnWebMessageReceived);
            Window.RegisterWindowCreatedHandler(SetupEvents);
            HandleHitTest = _handleHitTest;
        }
    }
}