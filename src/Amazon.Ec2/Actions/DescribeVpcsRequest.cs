namespace Amazon.Ec2;

public sealed class DescribeVpcsRequest : DescribeRequest, IEc2Request
{
    public DescribeVpcsRequest(params string[] vpcIds)
    {
        ArgumentNullException.ThrowIfNull(vpcIds);

        VpcIds = vpcIds;
    }

    public string[] VpcIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeVpcs");

        AddIds(parameters, "VpcId", VpcIds);

        return parameters;
    }
}
