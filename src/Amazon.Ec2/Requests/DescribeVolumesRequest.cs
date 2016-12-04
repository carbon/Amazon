using System.Collections.Generic;

namespace Amazon.Ec2
{
    public class DescribeVolumesRequest : DescribeRequest
    {        
        public List<string> VolumeIds { get; } = new List<string>();

        public AwsRequest ToParams()
        {
            var parameters = GetParameters("DescribeVolumes");

            var i = 1;

            foreach (var id in VolumeIds)
            {
                var prefix = "VolumeId." + i;

                parameters.Add("VolumeId." + i, id);

                i++;
            }

            return parameters;
        }
    }
}