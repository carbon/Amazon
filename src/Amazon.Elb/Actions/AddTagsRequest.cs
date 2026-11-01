using System.Diagnostics.CodeAnalysis;

namespace Amazon.Elb;

public sealed class AddTagsRequest : IElbRequest
{
    public AddTagsRequest() { }

    [SetsRequiredMembers]
    public AddTagsRequest(string[] resourceArns, Tag[] tags)
    {
        ArgumentNullException.ThrowIfNull(resourceArns);
        ArgumentNullException.ThrowIfNull(tags);

        ResourceArns = resourceArns;
        Tags = tags;
    }

    public string Action => "AddTags";

    public required string[] ResourceArns { get; init; }

    public required Tag[] Tags { get; init; }
}