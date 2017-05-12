using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeVolumesRequest : DescribeRequest, IEc2Request
    {        
        public List<string> VolumeIds { get; } = new List<string>();

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeVolumes");

            AddIds(parameters, "VolumeId", VolumeIds);

            return parameters;
        }
    }
}