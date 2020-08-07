using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SSEType
    {
        AES256 = 1,
        KMS = 2
    };
}