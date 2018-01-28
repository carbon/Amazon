using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeVolumesRequest : DescribeRequest, IEc2Request
    {
        public DescribeVolumesRequest() { }

        public DescribeVolumesRequest(string[] volumeIds)
        {
            VolumeIds = volumeIds;
        }

        public string[] VolumeIds { get; }

        public Dictionary<string, string> ToParams()
        {
            var parameters = GetParameters("DescribeVolumes");

            AddIds(parameters, "VolumeId", VolumeIds);

            return parameters;
        }
    }
}