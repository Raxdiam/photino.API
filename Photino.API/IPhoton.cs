using System.Text.Json.Serialization;

namespace PhotinoAPI
{
    public interface IPhoton<TData> where TData : IPhotonData
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        
        public TData Data { get; set; }
    }
}