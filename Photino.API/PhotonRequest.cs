using System.Text.Json.Serialization;

namespace PhotinoAPI
{
    public class PhotonRequest : IPhoton<PhotonRequestData>
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        
        public PhotonRequestData Data { get; set; }
    }
}