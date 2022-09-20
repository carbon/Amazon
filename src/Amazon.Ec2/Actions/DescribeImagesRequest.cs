namespace Amazon.Ec2;

public sealed class DescribeImagesRequest : DescribeRequest, IEc2Request
{
    public DescribeImagesRequest() { }

    public DescribeImagesRequest(string[] imageIds)
    {
        ArgumentNullException.ThrowIfNull(imageIds);

        ImageIds.AddRange(imageIds);
    }

    public List<string> ImageIds { get; } = new();

    public List<string> OwnerIds { get; } = new();

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeImages");

        AddIds(parameters, "ImageId", ImageIds);
        AddIds(parameters, "OwnerId", OwnerIds);

        return parameters;
    }
}
