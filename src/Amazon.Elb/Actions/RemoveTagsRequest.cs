#nullable disable

namespace Amazon.Elb;

public sealed class RemoveTagsRequest : IElbRequest
{
    public string Action => "RemoveTags";

    public string[] ResourceArns { get; init; }

    public string[] TagKeys { get; init; }
}