using System.Collections.Generic;

namespace PhotinoAPI
{
    public class PhotonRequestData : IPhotonData
    {
        public string Ns { get; set; }
        
        public string Action { get; set; }
        
        public Dictionary<string, object> Params { get; set; }
    }
}