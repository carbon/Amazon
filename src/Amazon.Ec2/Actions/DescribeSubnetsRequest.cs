namespace Amazon.Ec2;

public sealed class DescribeSubnetsRequest(params string[] subnetIds) : DescribeRequest, IEc2Request
{
    public string[] SubnetIds { get; } = subnetIds;

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeSubnets");

        AddIds(parameters, "SubnetId", SubnetIds);

        return parameters;
    }
}
