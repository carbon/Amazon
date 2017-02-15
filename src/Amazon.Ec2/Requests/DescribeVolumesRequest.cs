using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeVolumesRequest : DescribeRequest
    {        
        public List<string> VolumeIds { get; } = new List<string>();

        public AwsRequest ToParams()
        {
            var parameters = GetParameters("DescribeVolumes");

            AddIds(parameters, "VolumeId", VolumeIds);

            return parameters;
        }
    }
}