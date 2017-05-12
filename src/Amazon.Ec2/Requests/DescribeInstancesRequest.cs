using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeInstancesRequest : DescribeRequest, IEc2Request
    {
        public List<string> InstanceIds { get; } = new List<string>();

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeInstances");

            AddIds(parameters, "InstanceId", InstanceIds);

            return parameters;
        }
    }
}