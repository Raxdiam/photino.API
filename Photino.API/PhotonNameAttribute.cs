using System;

namespace PhotinoAPI
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PhotonNameAttribute : Attribute
    {
        public PhotonNameAttribute(string name) => Name = name;

        public string Name { get; set; }
    }
}