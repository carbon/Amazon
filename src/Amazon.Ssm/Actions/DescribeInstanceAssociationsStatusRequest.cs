namespace Amazon.Ssm
{
    public class DescribeInstanceAssociationsStatusRequest : ISsmRequest
    {
        public string InstanceId { get; set; }

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }
}