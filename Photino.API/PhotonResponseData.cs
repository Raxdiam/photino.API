namespace PhotinoAPI
{
    public class PhotonResponseData : IPhotonData
    {
        public object Return { get; set; }
        
        public string Message { get; set; }
        
        public PhotonStatus Status { get; set; }
    }
}