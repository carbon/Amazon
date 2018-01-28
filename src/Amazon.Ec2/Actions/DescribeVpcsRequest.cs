using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeVpcsRequest : DescribeRequest, IEc2Request
    {
        public DescribeVpcsRequest() { }

        public DescribeVpcsRequest(string[] vpcIds)
        {
            VpcIds = vpcIds;
        }

        public string[] VpcIds { get; }

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeVpcs");

            AddIds(parameters, "VpcId", VpcIds);

            return parameters;
        }
    }
}