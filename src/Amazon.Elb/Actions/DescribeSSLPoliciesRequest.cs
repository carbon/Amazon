namespace Amazon.Elb;

public sealed class DescribeSSLPoliciesRequest : IElbRequest
{
    public string Action => "DescribeSSLPolicies";

    public string? Marker { get; init; }

    public string[]? Names { get; init; }

    public int? PageSize { get; init; }
}

/*
https://elasticloadbalancing.amazonaws.com/?Action=DescribeSSLPolicies
&Names.member.1=ELBSecurityPolicy-2016-08
&Version=2015-12-01
&AUTHPARAMS
*/