using System.Collections.Generic;

namespace Amazon.Ec2;

public sealed class DescribeVolumesRequest : DescribeRequest, IEc2Request
{
    public DescribeVolumesRequest(params string[] volumeIds)
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
