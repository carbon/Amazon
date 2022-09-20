using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class StartInstancesRequest : IEc2Request
{
    public StartInstancesRequest(params string[] instanceIds)
    {
        ArgumentNullException.ThrowIfNull(instanceIds);

        InstanceIds = instanceIds;
    }

    public bool? DryRun { get; init; }

    [DataMember(Name = "InstanceId")]
    public string[] InstanceIds { get; }

    List<KeyValuePair<string, string>> IEc2Request.ToParams()
    {
        return Ec2RequestHelper.ToParams("StartInstances", this);
    }
}