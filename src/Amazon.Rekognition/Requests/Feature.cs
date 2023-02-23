using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Feature
{
    GENERAL_LABELS = 1,
    IMAGE_PROPERTIES = 2
}