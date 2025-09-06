using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class UpdateDestinationRequest
{
    [JsonPropertyName("CurrentDeliveryStreamVersionId")]
    public required string CurrentDeliveryStreamVersionId { get; init; }

    [JsonPropertyName("DeliveryStreamName")]
    [StringLength(64)]
    public required string DeliveryStreamName { get; init; }

    [JsonPropertyName("DestinationId")]
    [StringLength(100)]
    public required string DestinationId { get; init; }
}