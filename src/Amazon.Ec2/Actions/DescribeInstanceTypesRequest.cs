namespace Amazon.Ec2;

public sealed class DescribeInstanceTypesRequest : DescribeRequest, IEc2Request
{
    public DescribeInstanceTypesRequest(params string[] instanceTypes)
    {
        ArgumentNullException.ThrowIfNull(instanceTypes);

        InstanceTypes = instanceTypes;
    }

    public string[] InstanceTypes { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeInstanceTypes");

        AddIds(parameters, "InstanceType", InstanceTypes);

        return parameters;
    }
}