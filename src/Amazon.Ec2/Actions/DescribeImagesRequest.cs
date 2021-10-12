namespace Amazon.Ec2;

public sealed class DescribeImagesRequest : DescribeRequest, IEc2Request
{
    public DescribeImagesRequest() { }

    public DescribeImagesRequest(string[] imageIds)
    {
        ImageIds.AddRange(imageIds);
    }

    public List<string> ImageIds { get; } = new List<string>();

    public List<string> OwnerIds { get; } = new List<string>();

    public Dictionary<string, string> ToParams()
    {
        var parameters = GetParameters("DescribeImages");

        AddIds(parameters, "ImageId", ImageIds);
        AddIds(parameters, "OwnerId", OwnerIds);

        return parameters;
    }
}
