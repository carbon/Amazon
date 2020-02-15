namespace Amazon.Elb
{
    public sealed class DescribeTagsRequest : IElbRequest
    {
        public DescribeTagsRequest(string[] resourceArns)
        {
            ResourceArns = resourceArns;
        }

        public string Action => "DescribeTags";

        public string[] ResourceArns { get; }
    }
}