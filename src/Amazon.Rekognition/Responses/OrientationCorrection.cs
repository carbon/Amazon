using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

[JsonConverter(typeof(JsonStringEnumConverter<OrientationCorrection>))]
public enum OrientationCorrection
{
    ROTATE_0,
    ROTATE_90,
    ROTATE_180,
    ROTATE_270
}