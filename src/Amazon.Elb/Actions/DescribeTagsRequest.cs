namespace Amazon.Elb
{
    public class DescribeTagsRequest : IElbRequest
    {
        public string Action => "DescribeTags";

        public string[] ResourceArns { get; set; }
    }
}
