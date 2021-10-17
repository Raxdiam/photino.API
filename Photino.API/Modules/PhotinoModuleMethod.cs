using System;
using System.Reflection;

namespace PhotinoAPI.Modules
{
    internal class PhotinoModuleMethod
    {
        //private readonly Delegate _delegate;

        public PhotinoModuleMethod(MethodInfo info, Type moduleType, object moduleInstance = null)
        {
            (Info, ModuleType, ModuleInstance, RequireInstance) = (info, moduleType, moduleInstance, moduleInstance != null);

            /*var argTypes = info.GetParameters().Select(parameter => parameter.ParameterType).ToList();
            argTypes.Add(info.ReturnType);
            var delegateType = Expression.GetDelegateType(argTypes.ToArray());
            if (RequireInstance) {
                _delegate = Delegate.CreateDelegate(delegateType, ModuleInstance, Info);
            }
            else {
                _delegate = Delegate.CreateDelegate(delegateType, Info);
            }*/
        }
        
        public readonly MethodInfo Info;
        public readonly Type ModuleType;
        public readonly object ModuleInstance;
        public readonly bool RequireInstance;

        public object Execute(object[] args)
        {
            var parameters = Info.GetParameters();
            var allArgs = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++) {
                var param = parameters[i];
                if (i < args.Length)
                    allArgs[i] = args[i];
                else if (param.HasDefaultValue)
                    allArgs[i] = param.DefaultValue;
                else
                    allArgs[i] = default;
            }
            var result = Info.Invoke(RequireInstance ? ModuleInstance : null, allArgs);
            return result;
        }
    }
}