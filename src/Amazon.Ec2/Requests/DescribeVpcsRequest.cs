using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeVpcsRequest : DescribeRequest, IEc2Request
    {        
        public List<string> VpcIds { get; } = new List<string>();

        public AwsRequest ToParams()
        {
            var parameters = GetParameters("DescribeVpcs");

            AddIds(parameters, "VpcId", VpcIds);

            return parameters;
        }
    }
}