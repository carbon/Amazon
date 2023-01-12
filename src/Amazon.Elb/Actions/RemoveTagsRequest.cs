namespace Amazon.Elb;

public sealed class RemoveTagsRequest : IElbRequest
{
    public string Action => "RemoveTags";

    public required string[] ResourceArns { get; init; }

    public required string[] TagKeys { get; init; }
}