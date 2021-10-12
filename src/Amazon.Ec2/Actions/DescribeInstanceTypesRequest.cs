namespace Amazon.Ec2;

public sealed class DescribeInstanceTypesRequest : DescribeRequest, IEc2Request
{
    public DescribeInstanceTypesRequest(params string[] instanceTypes)
    {
        InstanceTypes = instanceTypes;
    }

    public string[] InstanceTypes { get; }

    public Dictionary<string, string> ToParams()
    {
        var parameters = GetParameters("DescribeInstanceTypes");

        AddIds(parameters, "InstanceType", InstanceTypes);

        return parameters;
    }
}