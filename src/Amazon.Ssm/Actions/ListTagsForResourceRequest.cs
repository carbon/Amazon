namespace Amazon.Ssm;

public sealed class ListTagsForResourceRequest : ISsmRequest
{
    public required string ResourceId { get; init; }

    // ManagedInstance | MaintenanceWindow | Parameter
    public required string ResourceType { get; init; }
}