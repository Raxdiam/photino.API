using System.Text.Json.Serialization;

namespace PhotinoAPI
{
    public class PhotonResponse : IPhoton<PhotonResponseData>
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        public PhotonResponseData Data { get; set; }
    }
}