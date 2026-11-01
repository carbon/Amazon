namespace Amazon.Elb;

public sealed class ModifyTargetGroupAttributesRequest : IElbRequest
{
    public string Action => "ModifyTargetGroupAttributes";

    public required TargetGroupAttribute[] Attributes { get; init; }

    public required string TargetGroupArn { get; init; }
}
