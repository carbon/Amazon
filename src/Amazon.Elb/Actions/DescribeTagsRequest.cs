namespace Amazon.Elb;

public sealed class DescribeTagsRequest(string[] resourceArns) : IElbRequest
{
    public string Action => "DescribeTags";

    public string[] ResourceArns { get; } = resourceArns;
}