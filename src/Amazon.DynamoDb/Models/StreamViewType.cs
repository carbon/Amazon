using System.Text.Json.Serialization;

namespace Amazon.DynamoDb
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StreamViewType
    {
        KEYS_ONLY = 1,
        NEW_IMAGE = 2,
        OLD_IMAGE = 3,
        NEW_AND_OLD_IMAGES = 4
    };
}
