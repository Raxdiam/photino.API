using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhotinoAPI.Modules;
using PhotinoAPI.Modules.Default;
using PhotinoNET;

namespace PhotinoAPI
{
    public class PhotinoApi
    {
        private static readonly string[] MethodBlacklist = { "GetType", "ToString", "Equals", "GetHashCode" };
        private static readonly JsonSerializerSettings JsonSettings = new() {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly Dictionary<string, Dictionary<string, PhotinoModuleMethod>> _moduleMap;
        
        private PhotinoWindow _window;
        private bool _handleHitTest;

        public PhotinoApi()
        {
            _moduleMap = new Dictionary<string, Dictionary<string, PhotinoModuleMethod>>();
        }
        
        public PhotinoWindow Window {
            get => _window;
            set => SetWindow(value);
        }

        public bool HandleHitTest {
            get => _handleHitTest;
            set => SetHandleHitTest(value);
        }

        public PhotinoApi SetWindow(PhotinoWindow window)
        {
            if (_window != null) {
                _window.WebMessageReceived -= OnApiModuleWebMessageReceived;
                if (_handleHitTest)
                    _window.WebMessageReceived -= OnHitTestWebMessageReceived;
                _moduleMap.Clear();
            }

            _window = window;
            
            RegisterModule(typeof(IOModule));
            RegisterModule(typeof(AppModule));
            RegisterModule(typeof(WindowModule));
            RegisterModule(typeof(OSModule));
            
            _window.WebMessageReceived += OnApiModuleWebMessageReceived;
            if (_handleHitTest)
                _window.WebMessageReceived += OnHitTestWebMessageReceived;

            return this;
        }
        
        public PhotinoApi SetHandleHitTest(bool value)
        {
            _handleHitTest = value;
            if (_window != null) {
                if (value) {
                    _window.WebMessageReceived -= OnHitTestWebMessageReceived;
                    _window.WebMessageReceived += OnHitTestWebMessageReceived;
                }
                else {
                    _window.WebMessageReceived -= OnHitTestWebMessageReceived;
                }
            }
            return this;
        }
        
        private void OnApiModuleWebMessageReceived(object sender, string e)
        {
            if (!e.StartsWith("api:")) return;

            var reqMsg = JsonConvert.DeserializeObject<PhotinoApiMessage<PhotinoApiRequest>>(e[4..], JsonSettings);
            if (reqMsg == null) return;

            var res = new PhotinoApiResponse();
            try {
                var result = ExecuteModuleMethod(reqMsg.Data.Module, reqMsg.Data.Method, reqMsg.Data.Parameters);
                res.Result = result;
            }
            catch (Exception ex) {
                res.Error = ex.InnerException?.Message;
            }

            var resMsg = new PhotinoApiMessage<PhotinoApiResponse> { Id = reqMsg.Id, Data = res };
            var resJson = JsonConvert.SerializeObject(resMsg, JsonSettings);
            Window.SendWebMessage($"api:{resJson}");
        }

        private void OnHitTestWebMessageReceived(object sender, string e)
        {
            if (!e.StartsWith("ht:")) return;

            var dir = e.Split(':')[1];
            var hitTest = dir switch {
                "se" => PhotinoHitTest.BottomRight,
                "e" => PhotinoHitTest.Right,
                "s" => PhotinoHitTest.Bottom,
                "ne" => PhotinoHitTest.TopRight,
                "n" => PhotinoHitTest.Top,
                "nw" => PhotinoHitTest.TopLeft,
                "w" => PhotinoHitTest.Left,
                "sw" => PhotinoHitTest.BottomLeft,
                _ => default
            };

            if (hitTest == default) return;

            PhotinoUtil.HitTest(_window, hitTest);
        }
        
        public object ExecuteModuleMethod(string moduleName, string methodName, params object[] args)
        {
            if (!_moduleMap.ContainsKey(moduleName)) {
                throw new Exception($"[{nameof(PhotinoApi)}.{nameof(ExecuteModuleMethod)}] Cannot find module '{moduleName}'");
            }

            var methodMap = _moduleMap[moduleName];
            if (!methodMap.ContainsKey(methodName)) {
                throw new Exception($"[{nameof(PhotinoApi)}.{nameof(ExecuteModuleMethod)}] Cannot find method '{methodMap}' in module '{moduleName}'");
            }
            
            return methodMap[methodName].Execute(args);
        }

        public PhotinoApi RegisterModule<T>() where T : IPhotinoModule
        {
            RegisterModule(typeof(T));
            return this;
        }

        private void RegisterModule(Type type)
        {
            var moduleName = PhotinoUtil.GetModuleName(type);

            var staticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var instMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            if (!staticMethods.Any() && !instMethods.Any()) return;

            if (_moduleMap.ContainsKey(moduleName))
                throw new Exception($"Module already exists with the name '{moduleName}'");

            _moduleMap.Add(moduleName, new Dictionary<string, PhotinoModuleMethod>());
            
            foreach (var staticMethod in staticMethods) {
                AddMethodToModuleMap(staticMethod, false);
            }
            
            foreach (var instMethod in instMethods) {
                if (MethodBlacklist.Contains(instMethod.Name)) continue;
                AddMethodToModuleMap(instMethod, true);
            }

            void AddMethodToModuleMap(MethodInfo info, bool instance)
            {
                var methodName = PhotinoUtil.GetMethodName(info);
                var method = new PhotinoModuleMethod(info, type, instance ? Activator.CreateInstance(type, this) : null);

                if (_moduleMap[moduleName].ContainsKey(methodName))
                    return;

                _moduleMap[moduleName].Add(methodName, method);
            }
        }

        private string TestModuleToTypeScript(Type type)
        {
            var moduleName = PhotinoUtil.GetModuleName(type);

            var staticMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            var instMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

            if (!staticMethods.Any() && !instMethods.Any()) return null;

            var allMethods = new List<MethodInfo>(staticMethods);
            allMethods.AddRange(instMethods);

            var builder = new StringBuilder();
            builder.AppendLine("import { PhotinoModule } from '../PhotinoModule';");
            builder.AppendLine();
            builder.AppendLine($"export class {type.Name} extends PhotinoModule {{");
            builder.AppendLine($"  readonly name: string = '{moduleName}';");
            builder.AppendLine();

            foreach (var methodInfo in allMethods) {
                var methodName = PhotinoUtil.GetMethodName(methodInfo);
                var parameters = methodInfo.GetParameters();
                var methodDef = parameters.Length > 0 ? parameters.Select(p => $"{p.Name}: {CsToTsType(p.ParameterType)}").Aggregate((s1, s2) => $"{s1}, {s2}") : "";
                var paramsDef = parameters.Length > 0 ? parameters.Select(p => p.Name.ToCamelCase()).Aggregate((s1, s2) => $"{s1}, {s2}") : "";
                builder.AppendLine($"  {methodName}({methodDef}): Promise<{CsToTsType(methodInfo.ReturnType)}> {{");
                builder.AppendLine($"    return this.send(this.{methodName}.name{(parameters.Length > 0 ? ", " : "")}{paramsDef});");
                builder.AppendLine("  }");
                builder.AppendLine();
            }

            builder.AppendLine("}");

            return builder.ToString();
        }

        private static string CsToTsType(Type type)
        {
            if (typeof(byte[]) == type) {
                return "Uint8Array";
            }

            switch (type.Name) {
                case nameof(String):
                    return "string";
                case nameof(Boolean):
                    return "boolean";
                case nameof(Single):
                case nameof(Int32):
                case nameof(Int64):
                case nameof(Double):
                    return "number";
                case "Void":
                    return "void";
            }

            return "void";
        }
    }
}
