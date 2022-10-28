namespace Amazon.Ec2;

public sealed class DescribeVolumesRequest : DescribeRequest, IEc2Request
{
    public DescribeVolumesRequest() { }

    public DescribeVolumesRequest(params string[] volumeIds)
    {
        Ensure.NotEmpty(volumeIds);

        VolumeIds = volumeIds;
    }

    public string[]? VolumeIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeVolumes");

        if (VolumeIds != null)
        {
            AddIds(parameters, "VolumeId", VolumeIds);
        }

        return parameters;
    }
}