namespace Amazon.Elb
{
    public class DescribeSSLPoliciesRequest : IElbRequest
    {
        public string Action => "DescribeSSLPolicies";

        public string Marker { get; set; }

        public string[] Names { get; set; }

        public int? PageSize { get; set; }
    }
}


/*
https://elasticloadbalancing.amazonaws.com/?Action=DescribeSSLPolicies
&Names.member.1=ELBSecurityPolicy-2016-08
&Version=2015-12-01
&AUTHPARAMS
*/