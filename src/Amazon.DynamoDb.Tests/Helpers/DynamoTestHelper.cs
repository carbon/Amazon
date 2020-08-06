using System.Text.Json;

namespace Amazon.DynamoDb.Models.Tests
{
    public static class DynamoTestHelper
    {
        public static readonly JsonSerializerOptions IndentedSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
            IgnoreNullValues = true,
        };

        public static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = false,
            IgnoreNullValues = true,
        };

        public static string ToSystemTextJson(this object obj)
        {
            return JsonSerializer.Serialize(obj, obj.GetType(), SerializerOptions);
        }

        public static string ToSystemTextJsonIndented(this object obj)
        {
            return JsonSerializer.Serialize(obj, obj.GetType(), IndentedSerializerOptions);
        }
    }
}