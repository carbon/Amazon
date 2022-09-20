namespace Amazon.Ec2;

public sealed class DescribeVolumesRequest : DescribeRequest, IEc2Request
{
    public DescribeVolumesRequest(params string[] volumeIds)
    {
        ArgumentNullException.ThrowIfNull(volumeIds);

        VolumeIds = volumeIds;
    }

    public string[] VolumeIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeVolumes");

        AddIds(parameters, "VolumeId", VolumeIds);

        return parameters;
    }
}
