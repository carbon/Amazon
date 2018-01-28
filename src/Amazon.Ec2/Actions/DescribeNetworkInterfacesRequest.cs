using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeNetworkInterfacesRequest : DescribeRequest, IEc2Request
    {
        public DescribeNetworkInterfacesRequest() { }

        public DescribeNetworkInterfacesRequest(string[] networkInterfaceIds)
        {
            NetworkInterfaceIds = networkInterfaceIds;
        }

        public string[] NetworkInterfaceIds { get; }

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeNetworkInterfaces");

            AddIds(parameters, "NetworkInterfaceId", NetworkInterfaceIds);

            return parameters;
        }
    }
}