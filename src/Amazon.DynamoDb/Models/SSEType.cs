using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SSEType : byte
    {
        AES256,
        KMS,
    };
}
