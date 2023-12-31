using System.Text.Json.Serialization;

namespace Amazon.Bedrock;

public sealed class TrainingMetrics
{
    [JsonPropertyName("trainingLoss")]
    public float TrainingLoss { get; init; }
}
