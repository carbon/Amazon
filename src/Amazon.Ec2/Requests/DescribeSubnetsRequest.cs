using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeSubnetsRequest : DescribeRequest, IEc2Request
    {        
        public List<string> SubnetIds { get; } = new List<string>();

        public AwsRequest ToParams()
        {
            var parameters = GetParameters("DescribeSubnets");

            AddIds(parameters, "SubnetId", SubnetIds);

            return parameters;
        }
    }
}