using System.Runtime.Serialization;

namespace Amazon.Ec2;

public sealed class RebootInstancesRequest : IEc2Request
{
    public RebootInstancesRequest(params string[] instanceIds!!)
    {
        InstanceIds = instanceIds;
    }

    public bool? DryRun { get; init; }

    [DataMember(Name = "InstanceId")]
    public string[] InstanceIds { get; }

    public Dictionary<string, string> ToParams()
    {
        return Ec2RequestHelper.ToParams("RebootInstances", this);
    }
}