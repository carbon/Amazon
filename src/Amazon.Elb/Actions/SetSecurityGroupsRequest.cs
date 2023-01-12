namespace Amazon.Elb;

public sealed class SetSecurityGroupsRequest : IElbRequest
{
    public string Action => "SetSecurityGroups";

    public required string LoadBalancerArn { get; init; }

    public required string[] SecurityGroups { get; init; }
}