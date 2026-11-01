using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class AddPermissionRequest : SqsRequest
{
    /// <param name="queueUrl">The URL of the queue</param>
    /// <param name="label">The unique name of the permission, up to 80 characters</param>
    /// <param name="awsAccountIds">The accounts that receive the permission</param>
    /// <param name="actions">The allowed action names, or *</param>
    public AddPermissionRequest(string queueUrl, string label, string[] awsAccountIds, string[] actions)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentException.ThrowIfNullOrEmpty(label);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(label.Length, 80, nameof(label));
        ArgumentNullException.ThrowIfNull(awsAccountIds);
        ArgumentNullException.ThrowIfNull(actions);

        QueueUrl = queueUrl;
        Label = label;
        AWSAccountIds = awsAccountIds;
        Actions = actions;
    }

    [JsonPropertyName("Actions")]
    public string[] Actions { get; }

    [JsonPropertyName("AWSAccountIds")]
    public string[] AWSAccountIds { get; }

    [JsonPropertyName("Label")]
    public string Label { get; }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }
}
