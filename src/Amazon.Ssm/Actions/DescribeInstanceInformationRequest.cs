namespace Amazon.Ssm
{
    public class DescribeInstanceInformationRequest : ISsmRequest
    {
        
        // Filters

        // InstanceInformationFilterList

        public int? MaxResults { get; set; }

        public string NextToken { get; set; }
    }

    // ref: http://docs.aws.amazon.com/systems-manager/latest/APIReference/API_DescribeInstanceInformation.html
}