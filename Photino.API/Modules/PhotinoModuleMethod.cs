using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PhotinoAPI.Modules
{
    internal class PhotinoModuleMethod
    {
        public PhotinoModuleMethod(MethodInfo info, Type moduleType, object moduleInstance = null) => 
            (Info, ModuleType, ModuleInstance, RequireInstance, IsAsync) = 
            (info, moduleType, moduleInstance, moduleInstance != null, IsAsyncMethod(info));

        public readonly MethodInfo Info;
        public readonly Type ModuleType;
        public readonly object ModuleInstance;
        public readonly bool RequireInstance;
        public readonly bool IsAsync;

        public object Execute(object[] args)
        {
            if (IsAsync) {
                return ExecuteAsync(args).GetAwaiter().GetResult();
            }

            return Info.Invoke(ModuleInstance, ResolveArguments(args));
        }

        public async Task<object> ExecuteAsync(object[] args)
        {
            var allArgs = ResolveArguments(args);
            if (IsAsync) {
                var task = (Task)Info.Invoke(ModuleInstance, allArgs);
                await task;
                var result = task.GetType().GetProperty("Result").GetValue(task);
                return result;
            }

            return await Task.Run(() => Info.Invoke(ModuleInstance, allArgs));
        }

        private object[] ResolveArguments(object[] args)
        {
            var parameters = Info.GetParameters();
            var allArgs = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                if (i < args.Length)
                    allArgs[i] = args[i];
                else if (param.HasDefaultValue)
                    allArgs[i] = param.DefaultValue;
                else
                    allArgs[i] = default;
            }

            return allArgs;
        }

        private static bool IsAsyncMethod(MethodInfo method)
        {
            var attType = typeof(AsyncStateMachineAttribute);
            var attrib = (AsyncStateMachineAttribute)method.GetCustomAttribute(attType);
            return attrib != null;
        }
    }
}