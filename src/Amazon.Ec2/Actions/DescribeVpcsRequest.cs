namespace Amazon.Ec2;

public sealed class DescribeVpcsRequest : DescribeRequest, IEc2Request
{
    public DescribeVpcsRequest(params string[] vpcIds)
    {
        ArgumentNullException.ThrowIfNull(vpcIds);

        VpcIds = vpcIds;
    }

    public string[] VpcIds { get; }

    public Dictionary<string, string> ToParams()
    {
        var parameters = GetParameters("DescribeVpcs");

        AddIds(parameters, "VpcId", VpcIds);

        return parameters;
    }
}
