using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PhotinoAPI.Modules;
using PhotinoAPI.Modules.Default;
using PhotinoNET;

namespace PhotinoAPI
{
    public class PhotinoApi
    {
        private static readonly string[] MethodBlacklist = { "GetType", "ToString", "Equals", "GetHashCode" };

        private readonly Dictionary<string, Dictionary<string, PhotinoModuleMethod>> _moduleMap;
        
        private PhotinoWindow _window;
        private bool _handleHitTest;
        private PhotinoApiEvents _events;

        public PhotinoApi()
        {
            _moduleMap = new Dictionary<string, Dictionary<string, PhotinoModuleMethod>>();
        }

        public PhotinoWindow Window {
            get => _window;
            set => SetWindow(value);
        }

        /// <summary>
        /// Get or set whether or not PhotinoAPI will handle hit-test messages for resizing and dragging the window.
        /// This will only work on Windows!
        /// Default: False
        /// </summary>
        public bool HandleHitTest {
            get => _handleHitTest;
            set => SetHandleHitTest(value);
        }

        /// <summary>
        /// Get or set whether to asynchronously execute module methods.
        /// Default: True
        /// </summary>
        public bool UseAsync { get; set; } = true;

        public PhotinoApi SetWindow(PhotinoWindow window)
        {
            if (_window != null) {
                _window.WebMessageReceived -= OnApiModuleWebMessageReceived;
                if (_handleHitTest)
                    _window.WebMessageReceived -= OnHitTestWebMessageReceived;
                _moduleMap.Clear();
                _events.Clear();
            }

            _window = window;
            
            RegisterModule(typeof(IOModule));
            RegisterModule(typeof(AppModule));
            RegisterModule(typeof(WindowModule));
            RegisterModule(typeof(OSModule));
            
            _window.WebMessageReceived += OnApiModuleWebMessageReceived;
            if (_handleHitTest)
                _window.WebMessageReceived += OnHitTestWebMessageReceived;

            _events = new PhotinoApiEvents(_window);

            return this;
        }

        /// <summary>
        /// Set whether or not PhotinoAPI will handle hit-test messages for resizing and dragging the window.
        /// This will only work on Windows!
        /// Default: False
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get or set whether to asynchronously execute module methods.
        /// Default: True
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public PhotinoApi SetUseAsync(bool value)
        {
            UseAsync = value;
            return this;
        }

        private async void OnApiModuleWebMessageReceived(object sender, string e)
        {
            if (!e.StartsWith("api:")) return;

            var reqMsg = JsonConvert.DeserializeObject<PhotinoApiMessage<PhotinoApiRequest>>(e[4..], PhotinoUtil.JsonSettings);
            if (reqMsg == null) return;

            var res = new PhotinoApiResponse();
            try {
                object result;
                if (UseAsync) {
                    result = await ExecuteModuleMethodAsync(reqMsg.Data.Module, reqMsg.Data.Method, reqMsg.Data.Parameters);
                }
                else {
                    result = ExecuteModuleMethod(reqMsg.Data.Module, reqMsg.Data.Method, reqMsg.Data.Parameters);
                }
                res.Result = result;
            }
            catch (Exception ex) {
                res.Error = ex.InnerException?.Message ?? e;
            }

            var resMsg = new PhotinoApiMessage<PhotinoApiResponse> { Id = reqMsg.Id, Data = res };
            var resJson = JsonConvert.SerializeObject(resMsg, PhotinoUtil.JsonSettings);
            Window.SendWebMessage($"api:{resJson}");
        }

        private void OnHitTestWebMessageReceived(object sender, string e)
        {
            if (!e.StartsWith("ht:")) return;

            var dir = e.Split(':')[1];
            var hitTest = dir switch {
                "drag" => HitTest.Drag,
                "left" => HitTest.Left,
                "right" => HitTest.Right,
                "top" => HitTest.Top,
                "topLeft" => HitTest.TopLeft,
                "topRight" => HitTest.TopRight,
                "bottom" => HitTest.Bottom,
                "bottomLeft" => HitTest.BottomLeft,
                "bottomRight" => HitTest.BottomRight,
                _ => default
            };

            if (hitTest == default) return;
            _window.PerformHitTest(hitTest);
        }
        
        public object ExecuteModuleMethod(string moduleName, string methodName, params object[] args)
        {
            ValidateModuleMethod(moduleName, methodName, out var methodMap);

            return methodMap[methodName].Execute(args);
        }

        public async Task<object> ExecuteModuleMethodAsync(string moduleName, string methodName, params object[] args)
        {
            ValidateModuleMethod(moduleName, methodName, out var methodMap);

            return await methodMap[methodName].ExecuteAsync(args);
        }

        private void ValidateModuleMethod(string moduleName, string methodName, out Dictionary<string, PhotinoModuleMethod> methodMap)
        {
            if (!_moduleMap.ContainsKey(moduleName)) {
                throw new Exception($"[{nameof(PhotinoApi)}.{nameof(ExecuteModuleMethod)}] Cannot find module '{moduleName}'");
            }

            methodMap = _moduleMap[moduleName];
            if (!methodMap.ContainsKey(methodName)) {
                throw new Exception($"[{nameof(PhotinoApi)}.{nameof(ExecuteModuleMethod)}] Cannot find method '{methodMap}' in module '{moduleName}'");
            }
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

        private static string TestModuleToTypeScript(Type type)
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

            return type.Name switch {
                nameof(String) => "string",
                nameof(Boolean) => "boolean",
                nameof(Single) or nameof(Int32) or nameof(Int64) or nameof(Double) => "number",
                "Void" => "void",
                _ => "void",
            };
        }
    }
}
