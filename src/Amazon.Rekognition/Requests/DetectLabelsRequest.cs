using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class DetectLabelsRequest : IRekognitionRequest
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Feature[]? Features { get; init; }

    public required Image Image { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxLabels { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public double? MinConfidence { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DetectLabelSettings? Settings { get; init; }
}