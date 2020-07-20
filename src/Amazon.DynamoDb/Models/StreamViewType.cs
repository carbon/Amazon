using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StreamViewType
    {
        KEYS_ONLY,
        NEW_IMAGE,
        OLD_IMAGE,
        NEW_AND_OLD_IMAGES,
    };
}
