#nullable disable

using System;

namespace Amazon.Elb;

public sealed class AddTagsRequest : IElbRequest
{
    public AddTagsRequest() { }

    public AddTagsRequest(string[] resourceArns, Tag[] tags)
    {
        ResourceArns = resourceArns ?? throw new ArgumentNullException(nameof(resourceArns));
        Tags = tags ?? throw new ArgumentNullException(nameof(tags));
    }

    public string Action => "AddTags";

    public string[] ResourceArns { get; init; }

    public Tag[] Tags { get; init; }
}