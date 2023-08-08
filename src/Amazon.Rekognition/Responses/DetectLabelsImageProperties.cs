using System.Text.Json.Serialization;

namespace Amazon.Rekognition;

public sealed class DetectLabelsImageProperties
{
    [JsonPropertyName("Background")]
    public DetectLabelsImageBackground? Background { get; set; }

    [JsonPropertyName("DominantColors")]
    public DominantColor[]? DominantColors { get; set; }

    [JsonPropertyName("Foreground")]
    public DetectLabelsImageBackground? Foreground { get; set; }

    [JsonPropertyName("Quality")]
    public DetectLabelsImageQuality? Quality { get; set; }
}