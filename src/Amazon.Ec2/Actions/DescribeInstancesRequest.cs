using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeInstancesRequest : DescribeRequest, IEc2Request
    {
        public DescribeInstancesRequest() { }

        public DescribeInstancesRequest(string[] instanceIds)
        {
            InstanceIds = instanceIds;
        }

        public string[] InstanceIds { get; }

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeInstances");

            AddIds(parameters, "InstanceId", InstanceIds);

            return parameters;
        }
    }
}