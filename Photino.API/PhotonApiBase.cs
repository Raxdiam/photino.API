using System.Runtime.InteropServices;
using PhotinoNET;

namespace PhotinoAPI
{
    public abstract class PhotonApiBase
    {
        protected PhotonApiBase(PhotonManager manager)
        {
            Manager = manager;
        }
        
        protected PhotonManager Manager { get; }

        protected PhotinoWindow Window => Manager.Window;
        
    }
}