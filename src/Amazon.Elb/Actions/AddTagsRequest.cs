#nullable disable

namespace Amazon.Elb;

public sealed class AddTagsRequest : IElbRequest
{
    public AddTagsRequest() { }

    public AddTagsRequest(string[] resourceArns!!, Tag[] tags!!)
    {
        ResourceArns = resourceArns;
        Tags = tags;
    }

    public string Action => "AddTags";

    public string[] ResourceArns { get; init; }

    public Tag[] Tags { get; init; }
}