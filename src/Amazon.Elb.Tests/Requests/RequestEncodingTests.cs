using System.Runtime.Serialization;

namespace Amazon.Elb.Tests;

public class RequestEncodingTests
{
    [Fact]
    public void AddTags()
    {
        var request = new AddTagsRequest {
            ResourceArns = ["a", "b"],
            Tags = [new Tag("k1", "v1"), new Tag { Key = "k2" }]
        };

        Assert.Equal(
            "Action=AddTags&ResourceArns.member.1=a&ResourceArns.member.2=b&Tags.member.1.Key=k1&Tags.member.1.Value=v1&Tags.member.2.Key=k2",
            Serializer.Serialize(request));
    }

    [Fact]
    public void AddTags_FromConstructor()
    {
        var request = new AddTagsRequest(["a"], [new Tag("k", "v")]);

        Assert.Equal("Action=AddTags&ResourceArns.member.1=a&Tags.member.1.Key=k&Tags.member.1.Value=v", Serializer.Serialize(request));
    }

    [Fact]
    public void RemoveTags()
    {
        var request = new RemoveTagsRequest { ResourceArns = ["a"], TagKeys = ["k1", "k2"] };

        Assert.Equal("Action=RemoveTags&ResourceArns.member.1=a&TagKeys.member.1=k1&TagKeys.member.2=k2", Serializer.Serialize(request));
    }

    [Fact]
    public void DescribeTags()
    {
        Assert.Equal("Action=DescribeTags&ResourceArns.member.1=a", Serializer.Serialize(new DescribeTagsRequest(["a"])));
    }

    [Fact]
    public void CreateListener_MatchesDocumentedParameters()
    {
        var request = new CreateListenerRequest {
            LoadBalancerArn = "lb",
            Protocol = Protocol.HTTPS,
            Port = 443,
            Certificates = [new Certificate("cert")],
            SslPolicy = "ELBSecurityPolicy-2016-08",
            DefaultActions = { new Action("tg") }
        };

        Assert.Equal(
            "Action=CreateListener&LoadBalancerArn=lb&Certificates.member.1.CertificateArn=cert" +
            "&DefaultActions.member.1.TargetGroupArn=tg&DefaultActions.member.1.Type=forward" +
            "&Port=443&Protocol=HTTPS&SslPolicy=ELBSecurityPolicy-2016-08",
            Serializer.Serialize(request));
    }

    [Fact]
    public void ModifyListener()
    {
        var request = new ModifyListenerRequest {
            ListenerArn = "l",
            Port = 8080,
            Protocol = Protocol.HTTP,
            DefaultActions = [new Action("tg")]
        };

        Assert.Equal(
            "Action=ModifyListener&DefaultActions.member.1.TargetGroupArn=tg&DefaultActions.member.1.Type=forward&ListenerArn=l&Port=8080&Protocol=HTTP",
            Serializer.Serialize(request));
    }

    [Fact]
    public void CreateLoadBalancer()
    {
        var request = new CreateLoadBalancerRequest("my-lb") {
            IpAddressType = "ipv4",
            Scheme = "internal",
            SecurityGroups = ["sg-1"],
            Subnets = ["s-1", "s-2"],
            Tags = [new Tag("k", "v")]
        };

        Assert.Equal(
            "Action=CreateLoadBalancer&IpAddressType=ipv4&Name=my-lb&Scheme=internal&SecurityGroups.member.1=sg-1" +
            "&Subnets.member.1=s-1&Subnets.member.2=s-2&Tags.member.1.Key=k&Tags.member.1.Value=v",
            Serializer.Serialize(request));
    }

    [Fact]
    public void CreateRule_WithoutConditions()
    {
        var request = new CreateRuleRequest {
            Actions = [new Action("tg")],
            Conditions = [],
            ListenerArn = "l",
            Priority = 10
        };

        Assert.Equal(
            "Action=CreateRule&Actions.member.1.TargetGroupArn=tg&Actions.member.1.Type=forward&ListenerArn=l&Priority=10",
            Serializer.Serialize(request));
    }

    [Fact]
    public void CreateTargetGroup()
    {
        var request = new CreateTargetGroupRequest {
            Name = "n",
            HealthCheckIntervalSeconds = 30,
            HealthCheckPath = "/health",
            HealthCheckPort = "traffic-port",
            HealthCheckProtocol = "HTTP",
            HealthCheckTimeoutSeconds = 5,
            HealthyThresholdCount = 3,
            Matcher = new Matcher("200-299"),
            Port = 80,
            Protocol = "HTTP",
            ProtocolVersion = "HTTP1",
            UnhealthyThresholdCount = 2,
            VpcId = "vpc-1"
        };

        Assert.Equal(
            "Action=CreateTargetGroup&HealthCheckIntervalSeconds=30&HealthCheckPath=/health&HealthCheckPort=traffic-port&HealthCheckProtocol=HTTP" +
            "&HealthCheckTimeoutSeconds=5&HealthyThresholdCount=3&Matcher.HttpCode=200-299&Name=n&Port=80&Protocol=HTTP&ProtocolVersion=HTTP1" +
            "&UnhealthyThresholdCount=2&VpcId=vpc-1",
            Serializer.Serialize(request));
    }

    [Fact]
    public void ModifyTargetGroup_OmitsUnsetValues()
    {
        var request = new ModifyTargetGroupRequest {
            TargetGroupArn = "tg",
            HealthCheckProtocol = "HTTPS",
            HealthCheckPort = "443",
            Matcher = new Matcher { HttpCode = "200" }
        };

        Assert.Equal(
            "Action=ModifyTargetGroup&HealthCheckPort=443&HealthCheckProtocol=HTTPS&Matcher.HttpCode=200&TargetGroupArn=tg",
            Serializer.Serialize(request));
    }

    [Fact]
    public void DeregisterTargets()
    {
        var request = new DeregisterTargetsRequest("tg", new TargetDescription("i-1", 80), new TargetDescription("i-2"));

        Assert.Equal(
            "Action=DeregisterTargets&TargetGroupArn=tg&Targets.member.1.Id=i-1&Targets.member.1.Port=80&Targets.member.2.Id=i-2",
            Serializer.Serialize(request));
    }

    [Fact]
    public void DescribeTargetHealth()
    {
        var request = new DescribeTargetHealthRequest {
            TargetGroupArn = "tg",
            Targets = [new TargetDescription("i-1", 80)]
        };

        Assert.Equal(
            "Action=DescribeTargetHealth&TargetGroupArn=tg&Targets.member.1.Id=i-1&Targets.member.1.Port=80",
            Serializer.Serialize(request));
    }

    [Fact]
    public void SetRulePriorities()
    {
        var request = new SetRulePrioritiesRequest([
            new RulePriorityPair { RuleArn = "r1", Priority = 5 },
            new RulePriorityPair { RuleArn = "r2" }
        ]);

        Assert.Equal(
            "Action=SetRulePriorities&RulePriorities.member.1.Priority=5&RulePriorities.member.1.RuleArn=r1&RulePriorities.member.2.RuleArn=r2",
            Serializer.Serialize(request));
    }

    [Fact]
    public void ModifyLoadBalancerAttributes()
    {
        var request = new ModifyLoadBalancerAttributesRequest {
            LoadBalancerArn = "lb",
            Attributes = [new LoadBalancerAttribute { Key = "idle_timeout.timeout_seconds", Value = "60" }]
        };

        Assert.Equal(
            "Action=ModifyLoadBalancerAttributes&Attributes.member.1.Key=idle_timeout.timeout_seconds&Attributes.member.1.Value=60&LoadBalancerArn=lb",
            Serializer.Serialize(request));
    }

    [Fact]
    public void SetIpAddressType()
    {
        Assert.Equal(
            "Action=SetIpAddressType&IpAddressType=dualstack&LoadBalancerArn=lb",
            Serializer.Serialize(new SetIpAddressTypeRequest("dualstack", "lb")));
    }

    [Fact]
    public void SetSecurityGroups()
    {
        Assert.Equal(
            "Action=SetSecurityGroups&LoadBalancerArn=lb&SecurityGroups.member.1=sg-1&SecurityGroups.member.2=sg-2",
            Serializer.Serialize(new SetSecurityGroupsRequest { LoadBalancerArn = "lb", SecurityGroups = ["sg-1", "sg-2"] }));
    }

    [Fact]
    public void DescribeRequests_EncodePagingAndFilters()
    {
        Assert.Equal(
            "Action=DescribeLoadBalancers&LoadBalancerArns.member.1=a&Marker=m&Names.member.1=n&PageSize=5",
            Serializer.Serialize(new DescribeLoadBalancersRequest { LoadBalancerArns = ["a"], Marker = "m", Names = ["n"], PageSize = 5 }));

        Assert.Equal(
            "Action=DescribeListeners&ListenerArns.member.1=a&LoadBalancerArn=lb&Marker=m&PageSize=5",
            Serializer.Serialize(new DescribeListenersRequest { ListenerArns = ["a"], LoadBalancerArn = "lb", Marker = "m", PageSize = 5 }));

        Assert.Equal(
            "Action=DescribeRules&ListenerArn=l&RuleArns.member.1=r&Marker=m",
            Serializer.Serialize(new DescribeRulesRequest { ListenerArn = "l", RuleArns = ["r"], Marker = "m" }));

        Assert.Equal(
            "Action=DescribeSSLPolicies&Marker=m&Names.member.1=n&PageSize=5",
            Serializer.Serialize(new DescribeSSLPoliciesRequest { Marker = "m", Names = ["n"], PageSize = 5 }));

        Assert.Equal(
            "Action=DescribeTargetGroups&LoadBalancerArn=lb&Marker=m&Names.member.1=n&PageSize=5&TargetGroupArns.member.1=a",
            Serializer.Serialize(new DescribeTargetGroupsRequest { LoadBalancerArn = "lb", Marker = "m", Names = ["n"], PageSize = 5, TargetGroupArns = ["a"] }));
    }

    [Fact]
    public void DescribeRequests_OmitUnsetValues()
    {
        Assert.Equal("Action=DescribeLoadBalancers", Serializer.Serialize(new DescribeLoadBalancersRequest()));
        Assert.Equal("Action=DescribeTargetGroups", Serializer.Serialize(new DescribeTargetGroupsRequest()));
    }

    [Theory]
    [InlineData(Protocol.HTTP, "HTTP")]
    [InlineData(Protocol.HTTPS, "HTTPS")]
    [InlineData(Protocol.TCP, "TCP")]
    [InlineData(Protocol.TLS, "TLS")]
    [InlineData(Protocol.UDP, "UDP")]
    [InlineData(Protocol.TCP_UDP, "TCP_UDP")]
    [InlineData(Protocol.GENEVE, "GENEVE")]
    public void Protocol_IsEncodedByName(Protocol protocol, string expected)
    {
        var request = new CreateListenerRequest { LoadBalancerArn = "lb", Protocol = protocol };

        Assert.Equal($"Action=CreateListener&LoadBalancerArn=lb&Protocol={expected}", Serializer.Serialize(request));
    }

    [Fact]
    public void ArnConstructors_RejectNullAndEmpty()
    {
        Func<string, object>[] constructors = [
            static v => new DeleteListenerRequest(v),
            static v => new DeleteLoadBalancerRequest(v),
            static v => new DeleteRuleRequest(v),
            static v => new DeleteTargetGroupRequest(v),
            static v => new DescribeLoadBalancerAttributesRequest(v),
            static v => new SetIpAddressTypeRequest(v, "lb"),
            static v => new SetIpAddressTypeRequest("ipv4", v),
            static v => new RegisterTargetsRequest(v, new TargetDescription("i-1")),
            static v => new DeregisterTargetsRequest(v, new TargetDescription("i-1")),
            static v => new CreateLoadBalancerRequest(v),
            static v => new Certificate(v),
            static v => new TargetDescription(v)
        ];

        foreach (var constructor in constructors)
        {
            Assert.Throws<ArgumentNullException>(() => constructor(null!));
            Assert.Throws<ArgumentException>(() => constructor(""));
        }

        Assert.Throws<ArgumentNullException>(() => new DescribeTargetGroupAttributesRequest(null!));
    }

    [Fact]
    public void CollectionConstructors_RejectNull()
    {
        Assert.Throws<ArgumentNullException>(() => new AddTagsRequest(null!, []));
        Assert.Throws<ArgumentNullException>(() => new AddTagsRequest([], null!));
        Assert.Throws<ArgumentNullException>(() => new RegisterTargetsRequest("tg", null!));
        Assert.Throws<ArgumentNullException>(() => new DeregisterTargetsRequest("tg", null!));
        Assert.Throws<ArgumentNullException>(() => new SetRulePrioritiesRequest(null!));
    }

    [Fact]
    public void Action_DefaultsToForward()
    {
        var action = new Action("tg");

        Assert.Equal("tg", action.TargetGroupArn);
        Assert.Equal("forward", action.Type);
        Assert.Equal("fixed-response", new Action("tg", "fixed-response").Type);
    }

    [Fact]
    public void LoadBalancerAttribute_ConstructorSetsValues()
    {
        var attribute = new LoadBalancerAttribute("k", "v");

        Assert.Equal("k", attribute.Key);
        Assert.Equal("v", attribute.Value);
    }

    private sealed class CustomModel
    {
        [DataMember(Name = "renamed")]
        public string Original => "a";

        [IgnoreDataMember]
        public string Ignored => "b";

        public string? Missing => null;

        public Matcher Nested => new("200");

        public List<int> Numbers => [1, 2];

        public string[] Empty => [];
    }

    [Fact]
    public void ToParams_HonorsDataMemberAttributes()
    {
        Assert.Equal(
            [
                new("renamed", "a"),
                new("Nested.HttpCode", "200"),
                new("Numbers.member.1", "1"),
                new("Numbers.member.2", "2")
            ],
            RequestHelper.ToParams(new CustomModel()));
    }

    private sealed class Outer
    {
        public Matcher Inner => new("200");
    }

    private sealed class Deep
    {
        public Outer[] Items => [new Outer()];
    }

    [Fact]
    public void ToParams_EncodesObjectsNestedInListElements()
    {
        Assert.Equal([new("Items.member.1.Inner.HttpCode", "200")], RequestHelper.ToParams(new Deep()));
    }

    private sealed class SelfReference
    {
        public int Value => 1;

        public SelfReference Self => this;
    }

    private sealed class Holder
    {
        public SelfReference Item => new();
    }

    [Fact]
    public void ToParams_StopsUnboundedRecursion()
    {
        var ex = Assert.Throws<Exception>(() => RequestHelper.ToParams(new Holder()));

        Assert.Equal("exceeded max depth of 8", ex.Message);
    }

    [Fact]
    public void CreateRule_EncodesConditionValues()
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_CreateRule.html
        var request = new CreateRuleRequest {
            Actions = [new Action("tg")],
            Conditions = [new RuleCondition { Field = "path-pattern", Values = ["/img/*", "/vid/*"] }],
            ListenerArn = "l",
            Priority = 10
        };

        Assert.Equal(
            "Action=CreateRule&Actions.member.1.TargetGroupArn=tg&Actions.member.1.Type=forward" +
            "&Conditions.member.1.Field=path-pattern&Conditions.member.1.Values.member.1=/img/*&Conditions.member.1.Values.member.2=/vid/*" +
            "&ListenerArn=l&Priority=10",
            Serializer.Serialize(request));
    }

    [Fact]
    public void ModifyRule_EncodesConditionValues()
    {
        var request = new ModifyRuleRequest {
            RuleArn = "r",
            Conditions = [new RuleCondition { Field = "host-header", Values = ["example.com"] }]
        };

        Assert.Equal(
            "Action=ModifyRule&Conditions.member.1.Field=host-header&Conditions.member.1.Values.member.1=example.com&RuleArn=r",
            Serializer.Serialize(request));
    }

    [Fact]
    public void SetSubnets_MatchesDocumentedParameters()
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_SetSubnets.html
        var request = new SetSubnetsRequest { LoadBalancerArn = "lb", Subnets = ["s-1", "s-2"] };

        Assert.Equal("Action=SetSubnets&LoadBalancerArn=lb&Subnets.member.1=s-1&Subnets.member.2=s-2", Serializer.Serialize(request));
    }

    [Fact]
    public void ModifyTargetGroupAttributes_MatchesDocumentedParameters()
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_ModifyTargetGroupAttributes.html
        var request = new ModifyTargetGroupAttributesRequest {
            TargetGroupArn = "tg",
            Attributes = [new TargetGroupAttribute { Key = "deregistration_delay.timeout_seconds", Value = "600" }]
        };

        Assert.Equal(
            "Action=ModifyTargetGroupAttributes&Attributes.member.1.Key=deregistration_delay.timeout_seconds&Attributes.member.1.Value=600&TargetGroupArn=tg",
            Serializer.Serialize(request));
    }

    [Fact]
    public void CreateTargetGroup_OmitsUnsetHealthCheckValues()
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_CreateTargetGroup.html
        Assert.Equal("Action=CreateTargetGroup&Name=n", Serializer.Serialize(new CreateTargetGroupRequest { Name = "n" }));
    }

    [Fact]
    public void ModifyTargetGroup_AcceptsTrafficPort()
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_ModifyTargetGroup.html
        var request = new ModifyTargetGroupRequest { TargetGroupArn = "tg", HealthCheckPort = "traffic-port" };

        Assert.Equal("Action=ModifyTargetGroup&HealthCheckPort=traffic-port&TargetGroupArn=tg", Serializer.Serialize(request));
    }

    [Fact]
    public void RegisterTargets_SupportsMoreThan100Targets()
    {
        var targets = Enumerable.Range(1, 150).Select(static i => new TargetDescription($"i-{i}")).ToArray();

        var parameters = RequestHelper.ToParams(new RegisterTargetsRequest("tg", targets));

        Assert.Equal(152, parameters.Count);
        Assert.Equal(new("Targets.member.150.Id", "i-150"), parameters[^1]);
    }
}
