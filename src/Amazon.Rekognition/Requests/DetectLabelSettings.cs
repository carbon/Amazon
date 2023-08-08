using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class DetectLabelSettings
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GeneralLabelsSettings? GeneralLabels { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DetectLabelsImagePropertiesSettings? ImageProperties { get; init; }
}