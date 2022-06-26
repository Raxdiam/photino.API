using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PhotinoNET;

namespace PhotinoAPI
{
    public static class PhotinoExtensions
    {
        internal static readonly JsonSerializerSettings JsonSettings = new() {
            ContractResolver = new DefaultContractResolver {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public static PhotinoWindow SetUseApi(this PhotinoWindow window)
        {
            PhotinoApi.Init(window);
            return window;
        }

        internal static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value, JsonSettings);
        }

        internal static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, JsonSettings);
        }
    }
}
