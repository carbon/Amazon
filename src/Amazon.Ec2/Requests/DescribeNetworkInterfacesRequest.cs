using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeNetworkInterfacesRequest : DescribeRequest, IEc2Request
    {        
        public List<string> NetworkInterfaceIds { get; } = new List<string>();

        public AwsRequest ToParams()
        {
            var parameters = GetParameters("DescribeNetworkInterfaces");

            AddIds(parameters, "NetworkInterfaceId", NetworkInterfaceIds);

            return parameters;
        }
    }
}