namespace Amazon.Ssm;

public sealed class AddTagsToResourceRequest : ISsmRequest
{
    public required string ResourceId { get; set; }

    public required string ResourceType { get; set; }

    public required Tag[] Tags { get; set; }
}