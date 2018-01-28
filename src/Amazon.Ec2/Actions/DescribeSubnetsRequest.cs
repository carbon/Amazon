using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeSubnetsRequest : DescribeRequest, IEc2Request
    {
        public DescribeSubnetsRequest() { }

        public DescribeSubnetsRequest(string[] subnetIds)
        {
            SubnetIds = subnetIds;
        }

        public string[] SubnetIds { get; }

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeSubnets");

            AddIds(parameters, "SubnetId", SubnetIds);

            return parameters;
        }
    }
}