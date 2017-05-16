namespace Amazon.Ssm
{
    public class DescribeActivationsRequest : ISsmRequest
    {
        public DescribeActivationsFilter[] Filters { get; set; }

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }
}