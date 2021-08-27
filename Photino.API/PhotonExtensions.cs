using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using PhotinoNET;

namespace PhotinoAPI
{
    public static class PhotonExtensions
    {
        public static PhotinoWindow RegisterPhotonManager(this PhotinoWindow window, PhotonManager manager)
        {
            manager.SetWindow(window);
            return window;
        }

        public static PhotinoWindow LoadResource(this PhotinoWindow window, string name)
        {
            window.LoadRawString(PhotonUtil.GetResourceString(Assembly.GetCallingAssembly(), name));
            return window;
        }
    }

    internal static class Extensions
    {
        public static T ToObject<T>(this JsonElement element, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter)) {
                element.WriteTo(writer);
            }

            return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
        }

        public static T ToObject<T>(this JsonDocument document, JsonSerializerOptions options = null)
        {
            if (document == null) {
                throw new ArgumentNullException(nameof(document));
            }

            return document.RootElement.ToObject<T>(options);
        }

        public static object ToObject(this JsonElement element, Type returnType, JsonSerializerOptions options = null)
        {
            var bufferWriter = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(bufferWriter)) {
                element.WriteTo(writer);
            }

            return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
        }

        public static object ToObject(this JsonDocument document, Type returnType, JsonSerializerOptions options = null)
        {
            if (document == null) {
                throw new ArgumentNullException(nameof(document));
            }

            return document.RootElement.ToObject(returnType, options);
        }

        public static string Join(this IEnumerable<string> collection, string seperator) => collection.Aggregate((s1, s2) => $"{s1}{seperator}{s2}");

        public static bool IsVoid(this string value)
        {
            return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
        }
    }
}