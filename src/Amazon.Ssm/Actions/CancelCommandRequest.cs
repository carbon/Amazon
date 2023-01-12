namespace Amazon.Ssm;

public sealed class CancelCommandRequest : ISsmRequest
{
    public required string CommandId { get; init; }

    // If empty, cancels all
    public string[]? InstanceIds { get; init; }
}
