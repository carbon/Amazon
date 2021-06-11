using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    internal static class DynamoTestHelper
    {
        public static readonly JsonSerializerOptions IndentedSerializerOptions = new () {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static readonly JsonSerializerOptions SerializerOptions = new () {
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public static string ToSystemTextJson<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, SerializerOptions);
        }

        public static string ToSystemTextJsonIndented<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, IndentedSerializerOptions);
        }
    }
}