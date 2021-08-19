using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using PhotinoAPI.Ns;
using PhotinoNET;

namespace PhotinoAPI
{
    public class PhotonManager
    {
        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, IgnoreNullValues = true};
        private static readonly Dictionary<string, Dictionary<string, MethodInfo>> methodMap = new();
        private static readonly Dictionary<string, PhotonApiBase> instMap = new();

        public PhotonManager()
        {
            Register<IO>();
            Register<OS>();
            Register<App>();
            Register<Window>();
        }

        public PhotinoWindow Window { get; private set; }
        
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

        private void OnWebMessageReceived(object sender, string message)
        {
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

        private void SendError(ref PhotonResponse res, string message) => Send(ref res, PhotonStatus.Error, message: message);
        private void SendError(ref PhotonResponse res, Exception ex) => SendError(ref res, $"{ex.Message}\n{ex.StackTrace}");

        private void SendSuccess(ref PhotonResponse res, object ret) => Send(ref res, PhotonStatus.Success, ret);

        private void Send(ref PhotonResponse res, PhotonStatus status, object ret = null, string message = null)
        {
            res.Data.Status = status;
            res.Data.Return = ret;
            res.Data.Message = message;
            Window.SendWebMessage(JsonSerializer.Serialize(res, jsonOptions));
        }
        
        internal void SetWindow(PhotinoWindow window)
        {
            Window = window;
            Window.RegisterWebMessageReceivedHandler(OnWebMessageReceived);
        }
    }
}