namespace Amazon.Ec2;

public sealed class DescribeInstancesRequest : DescribeRequest, IEc2Request
{
    public DescribeInstancesRequest(params string[] instanceIds)
    {
        ArgumentNullException.ThrowIfNull(instanceIds);

        InstanceIds = instanceIds;
    }

    public string[] InstanceIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        var parameters = GetParameters("DescribeInstances");

        AddIds(parameters, "InstanceId", InstanceIds);

        return parameters;
    }
}