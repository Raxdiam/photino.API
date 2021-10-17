using System.Text.RegularExpressions;

namespace PhotinoAPI
{
    internal static class Extensions
    {
        private static readonly Regex CamelCaseRegex = new Regex("([A-Z])([A-Z]+)($|[A-Z])", RegexOptions.Compiled);

        public static string ToLowerFirstChar(this string value)
        {
            if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
                return value;

            return char.ToLower(value[0]) + value[1..];
        }

        public static string ToCamelCase(this string value)
        {
            var x = value.Replace("_", "");
            if (x.Length == 0) return "null";
            x = CamelCaseRegex.Replace(x, m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
            return char.ToLower(x[0]) + x[1..];
        }
    }
}
