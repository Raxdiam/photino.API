using System;

namespace PhotinoAPI.Modules
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PhotinoNameAttribute : Attribute
    {
        public PhotinoNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
