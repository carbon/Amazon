namespace Amazon.Ssm;

public sealed class RemoveTagsFromResourceRequest : ISsmRequest
{
    public required string ResourceId { get; init; }

    public required string ResourceType { get; init; }

    public required string[] TagKeys { get; init; }
}