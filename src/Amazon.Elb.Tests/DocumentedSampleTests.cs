using System.Net;

using Amazon.Elb.Exceptions;

namespace Amazon.Elb.Tests;

// Requests, responses and errors taken from the samples at
// https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_{Action}.html
public class DocumentedSampleTests
{
    private const string LbArn = "arn:aws:elasticloadbalancing:us-west-2:123456789012:loadbalancer/app/my-load-balancer/50dc6c495c0c9188";
    private const string TgArn = "arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067";
    private const string ListenerArn = "arn:aws:elasticloadbalancing:us-west-2:123456789012:listener/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2";
    private const string RuleArn = "arn:aws:elasticloadbalancing:us-west-2:123456789012:listener-rule/app/my-load-balancer/50dc6c495c0c9188/f2f7dc8efc522ab2/9683b2d02a6cabee";

    // The docs do not order parameters the way the request models do, and order is not significant
    private static void AssertSameParameters(string documented, IElbRequest request)
    {
        Assert.Equal(
            documented.Split('&').Order(StringComparer.Ordinal),
            Serializer.Serialize(request).Split('&').Order(StringComparer.Ordinal));
    }

    [Fact]
    public void AddTags()
    {
        AssertSameParameters(
            $"Action=AddTags&ResourceArns.member.1={LbArn}&Tags.member.1.Key=project&Tags.member.1.Value=lima&Tags.member.2.Key=department&Tags.member.2.Value=digital-media",
            new AddTagsRequest([LbArn], [new Tag("project", "lima"), new Tag("department", "digital-media")]));
    }

    [Fact]
    public void RemoveTags()
    {
        AssertSameParameters(
            $"Action=RemoveTags&ResourceArns.member.1={LbArn}&TagKeys.member.1=project&TagKeys.member.2=department",
            new RemoveTagsRequest { ResourceArns = [LbArn], TagKeys = ["project", "department"] });
    }

    [Theory]
    [InlineData(Protocol.HTTP, 80)]
    [InlineData(Protocol.TCP, 80)]
    [InlineData(Protocol.UDP, 53)]
    [InlineData(Protocol.TCP_UDP, 80)]
    public void CreateListener_Forward(Protocol protocol, int port)
    {
        AssertSameParameters(
            $"Action=CreateListener&LoadBalancerArn={LbArn}&Protocol={protocol}&Port={port}&DefaultActions.member.1.Type=forward&DefaultActions.member.1.TargetGroupArn={TgArn}",
            new CreateListenerRequest {
                LoadBalancerArn = LbArn,
                Protocol = protocol,
                Port = (ushort)port,
                DefaultActions = { new Action(TgArn) }
            });
    }

    [Theory]
    [InlineData(Protocol.HTTPS)]
    [InlineData(Protocol.TLS)]
    public void CreateListener_Secure(Protocol protocol)
    {
        const string certificate = "arn:aws:iam::123456789012:server-certificate/my-server-cert";

        AssertSameParameters(
            $"Action=CreateListener&LoadBalancerArn={LbArn}&Protocol={protocol}&Port=443&Certificates.member.1.CertificateArn={certificate}" +
            $"&SslPolicy=ELBSecurityPolicy-2016-08&DefaultActions.member.1.Type=forward&DefaultActions.member.1.TargetGroupArn={TgArn}",
            new CreateListenerRequest {
                LoadBalancerArn = LbArn,
                Protocol = protocol,
                Port = 443,
                Certificates = [new Certificate(certificate)],
                SslPolicy = "ELBSecurityPolicy-2016-08",
                DefaultActions = { new Action(TgArn) }
            });
    }

    [Fact]
    public void CreateLoadBalancer_Internal()
    {
        AssertSameParameters(
            "Action=CreateLoadBalancer&Name=my-internal-load-balancer&Scheme=internal&Subnets.member.1=subnet-8360a9e7&Subnets.member.2=subnet-b7d581c0",
            new CreateLoadBalancerRequest("my-internal-load-balancer") { Scheme = "internal", Subnets = ["subnet-8360a9e7", "subnet-b7d581c0"] });
    }

    [Fact]
    public void CreateRule()
    {
        AssertSameParameters(
            $"Action=CreateRule&ListenerArn={ListenerArn}&Priority=10&Conditions.member.1.Field=path-pattern&Conditions.member.1.Values.member.1=/img/*" +
            $"&Actions.member.1.Type=forward&Actions.member.1.TargetGroupArn={TgArn}",
            new CreateRuleRequest {
                ListenerArn = ListenerArn,
                Priority = 10,
                Conditions = [new RuleCondition { Field = "path-pattern", Values = ["/img/*"] }],
                Actions = [new Action(TgArn)]
            });
    }

    [Fact]
    public void DeleteRequests()
    {
        AssertSameParameters($"Action=DeleteListener&ListenerArn={ListenerArn}", new DeleteListenerRequest(ListenerArn));
        AssertSameParameters($"Action=DeleteLoadBalancer&LoadBalancerArn={LbArn}", new DeleteLoadBalancerRequest(LbArn));
    }

    [Fact]
    public void DescribeRequests()
    {
        AssertSameParameters($"Action=DescribeListeners&ListenerArns.member.1={ListenerArn}", new DescribeListenersRequest { ListenerArns = [ListenerArn] });
        AssertSameParameters($"Action=DescribeLoadBalancerAttributes&LoadBalancerArn={LbArn}", new DescribeLoadBalancerAttributesRequest(LbArn));
        AssertSameParameters($"Action=DescribeLoadBalancers&LoadBalancerArns.member.1={LbArn}", new DescribeLoadBalancersRequest { LoadBalancerArns = [LbArn] });
        AssertSameParameters($"Action=DescribeRules&RuleArns.member.1={RuleArn}", new DescribeRulesRequest { RuleArns = [RuleArn] });
        AssertSameParameters($"Action=DescribeRules&ListenerArn={ListenerArn}", new DescribeRulesRequest { ListenerArn = ListenerArn });
        AssertSameParameters("Action=DescribeSSLPolicies&Names.member.1=ELBSecurityPolicy-2016-08", new DescribeSSLPoliciesRequest { Names = ["ELBSecurityPolicy-2016-08"] });
        AssertSameParameters($"Action=DescribeTargetGroupAttributes&TargetGroupArn={TgArn}", new DescribeTargetGroupAttributesRequest(TgArn));
        AssertSameParameters($"Action=DescribeTargetGroups&TargetGroupArns.member.1={TgArn}", new DescribeTargetGroupsRequest { TargetGroupArns = [TgArn] });
        AssertSameParameters($"Action=DescribeTargetGroups&LoadBalancerArn={LbArn}", new DescribeTargetGroupsRequest { LoadBalancerArn = LbArn });
        AssertSameParameters($"Action=DescribeTargetHealth&TargetGroupArn={TgArn}", new DescribeTargetHealthRequest { TargetGroupArn = TgArn });
    }

    [Fact]
    public void ModifyListener_Variants()
    {
        const string certificate = "arn:aws:iam::123456789012:server-certificate/my-new-server-cert";

        AssertSameParameters(
            $"Action=ModifyListener&ListenerArn={ListenerArn}&DefaultActions.member.1.Type=forward&DefaultActions.member.1.TargetGroupArn={TgArn}",
            new ModifyListenerRequest { ListenerArn = ListenerArn, DefaultActions = [new Action(TgArn)] });

        AssertSameParameters(
            $"Action=ModifyListener&ListenerArn={ListenerArn}&Certificates.member.1.CertificateArn={certificate}",
            new ModifyListenerRequest { ListenerArn = ListenerArn, Certificates = [new Certificate(certificate)] });

        AssertSameParameters(
            $"Action=ModifyListener&ListenerArn={ListenerArn}&SslPolicy=ELBSecurityPolicy-2016-08",
            new ModifyListenerRequest { ListenerArn = ListenerArn, SslPolicy = "ELBSecurityPolicy-2016-08" });
    }

    [Fact]
    public void ModifyLoadBalancerAttributes_MultipleAttributes()
    {
        AssertSameParameters(
            $"Action=ModifyLoadBalancerAttributes&LoadBalancerArn={LbArn}&Attributes.member.1.Key=access_logs.s3.enabled&Attributes.member.1.Value=true" +
            "&Attributes.member.2.Key=access_logs.s3.bucket&Attributes.member.2.Value=my-loadbalancer-logs" +
            "&Attributes.member.3.Key=access_logs.s3.prefix&Attributes.member.3.Value=myapp",
            new ModifyLoadBalancerAttributesRequest {
                LoadBalancerArn = LbArn,
                Attributes = [
                    new LoadBalancerAttribute("access_logs.s3.enabled", "true"),
                    new LoadBalancerAttribute("access_logs.s3.bucket", "my-loadbalancer-logs"),
                    new LoadBalancerAttribute("access_logs.s3.prefix", "myapp")
                ]
            });
    }

    [Fact]
    public void RegisterTargets_SameInstanceOnTwoPorts()
    {
        AssertSameParameters(
            $"Action=RegisterTargets&TargetGroupArn={TgArn}&Targets.member.1.Id=i-80c8dd94&Targets.member.1.Port=80&Targets.member.2.Id=i-80c8dd94&Targets.member.2.Port=766",
            new RegisterTargetsRequest(TgArn, new TargetDescription("i-80c8dd94", 80), new TargetDescription("i-80c8dd94", 766)));
    }

    [Fact]
    public void RegisterTargets_LambdaFunction()
    {
        const string function = "aws:lambda:us-west-2:123456789012:function:my-function";

        AssertSameParameters(
            $"Action=RegisterTargets&TargetGroupArn={TgArn}&Targets.member.1.Id={function}",
            new RegisterTargetsRequest(TgArn, new TargetDescription(function)));
    }

    [Fact]
    public void SetRulePriorities()
    {
        AssertSameParameters(
            $"Action=SetRulePriorities&RulePriorities.member.1.RuleArn={RuleArn}&RulePriorities.member.1.Priority=5",
            new SetRulePrioritiesRequest([new RulePriorityPair { RuleArn = RuleArn, Priority = 5 }]));
    }

    [Fact]
    public void DescribeTargetHealth_Response()
    {
        var response = ElbSerializer<DescribeTargetHealthResponse>.DeserializeXml(
            """
            <DescribeTargetHealthResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <DescribeTargetHealthResult>
                <TargetHealthDescriptions>
                  <member>
                    <HealthCheckPort>80</HealthCheckPort>
                    <TargetHealth>
                      <State>healthy</State>
                    </TargetHealth>
                    <Target>
                      <Port>80</Port>
                      <Id>i-0f76fade</Id>
                    </Target>
                  </member>
                </TargetHealthDescriptions>
              </DescribeTargetHealthResult>
              <ResponseMetadata>
                <RequestId>c534f810-f389-11e5-9192-3fff33344cfa</RequestId>
              </ResponseMetadata>
            </DescribeTargetHealthResponse>
            """u8.ToArray());

        var description = Assert.Single(response.DescribeTargetHealthResult.TargetHealthDescriptions);

        Assert.Equal("healthy", description.TargetHealth.State);
        Assert.Null(description.TargetHealth.Reason);
        Assert.Equal("i-0f76fade", description.Target.Id);
        Assert.Equal(80, description.Target.Port);
    }

    [Fact]
    public void DescribeTags_Response_ValueBeforeKey()
    {
        var response = ElbSerializer<DescribeTagsResponse>.DeserializeXml(
            """
            <DescribeTagsResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <DescribeTagsResult>
                <TagDescriptions>
                  <member>
                    <ResourceArn>arn</ResourceArn>
                    <Tags>
                      <member>
                        <Value>lima</Value>
                        <Key>project</Key>
                      </member>
                      <member>
                        <Value>digital-media</Value>
                        <Key>department</Key>
                      </member>
                    </Tags>
                  </member>
                </TagDescriptions>
              </DescribeTagsResult>
            </DescribeTagsResponse>
            """u8.ToArray());

        var tags = Assert.Single(response.DescribeTagsResult.TagDescriptions).Tags;

        Assert.Equal(["project", "department"], tags.Select(static t => t.Key));
        Assert.Equal(["lima", "digital-media"], tags.Select(static t => t.Value));
    }

    [Fact]
    public void SetIpAddressType_Response()
    {
        var response = ElbSerializer<SetIpAddressTypeResponse>.DeserializeXml(
            """
            <SetIpAddressTypeResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <SetIpAddressTypeResult>
                <IpAddressType>dualstack</IpAddressType>
              </SetIpAddressTypeResult>
            </SetIpAddressTypeResponse>
            """u8.ToArray());

        Assert.Equal("dualstack", response.SetIpAddressTypeResult.IpAddressType);
    }

    [Fact]
    public void CreateListener_Response_ReadsCertificatesAndSslPolicy()
    {
        var response = ElbSerializer<CreateListenerResponse>.DeserializeXml(
            """
            <CreateListenerResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <CreateListenerResult>
                <Listeners>
                  <member>
                    <LoadBalancerArn>lb</LoadBalancerArn>
                    <Protocol>HTTPS</Protocol>
                    <Certificates>
                      <member>
                        <CertificateArn>arn:aws:iam::123456789012:server-certificate/my-server-cert</CertificateArn>
                      </member>
                    </Certificates>
                    <Port>443</Port>
                    <ListenerArn>l</ListenerArn>
                    <SslPolicy>ELBSecurityPolicy-2016-08</SslPolicy>
                  </member>
                </Listeners>
              </CreateListenerResult>
            </CreateListenerResponse>
            """u8.ToArray());

        var listener = Assert.Single(response.CreateListenerResult.Listeners);

        Assert.Equal("arn:aws:iam::123456789012:server-certificate/my-server-cert", Assert.Single(listener.Certificates).CertificateArn);
        Assert.Equal("ELBSecurityPolicy-2016-08", listener.SslPolicy);
    }

    [Fact]
    public void Listener_WithoutCertificates_HasNoSslDetails()
    {
        var response = ElbSerializer<CreateListenerResponse>.DeserializeXml(
            """
            <CreateListenerResponse xmlns="http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/">
              <CreateListenerResult><Listeners><member><Protocol>HTTP</Protocol></member></Listeners></CreateListenerResult>
            </CreateListenerResponse>
            """u8.ToArray());

        Assert.Null(response.CreateListenerResult.Listeners[0].Certificates);
        Assert.Null(response.CreateListenerResult.Listeners[0].SslPolicy);
    }

    [Theory]
    [InlineData("LoadBalancerNotFound")]
    [InlineData("ListenerNotFound")]
    [InlineData("RuleNotFound")]
    [InlineData("TargetGroupNotFound")]
    [InlineData("TooManyTargets")]
    [InlineData("InvalidConfigurationRequest")]
    public async Task DocumentedErrorCodes_SurfaceOnElbException(string code)
    {
        var handler = new FakeElbHandler(
            $"""<ErrorResponse xmlns="{ElbClient.Namespace}"><Error><Type>Sender</Type><Code>{code}</Code><Message>m</Message></Error></ErrorResponse>""",
            HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<ElbException>(() => FakeElb.Create(handler).DescribeLoadBalancersAsync(new DescribeLoadBalancersRequest()));

        Assert.Equal(code, ex.Error.Code);
        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
    }

    [Fact]
    public void CreateListener_GatewayLoadBalancer_HasNoProtocolOrPort()
    {
        AssertSameParameters(
            $"Action=CreateListener&LoadBalancerArn={LbArn}&DefaultActions.member.1.Type=forward&DefaultActions.member.1.TargetGroupArn={TgArn}",
            new CreateListenerRequest { LoadBalancerArn = LbArn, DefaultActions = { new Action(TgArn) } });
    }

    [Theory]
    [InlineData("Protocol=HTTP&Port=80&VpcId=vpc-1&TargetType=instance", "HTTP", 80, "vpc-1", "instance")]
    [InlineData("Protocol=TCP&Port=80&VpcId=vpc-1&TargetType=ip", "TCP", 80, "vpc-1", "ip")]
    [InlineData("Protocol=GENEVE&Port=6081&VpcId=vpc-1&TargetType=ip", "GENEVE", 6081, "vpc-1", "ip")]
    public void CreateTargetGroup_TargetTypes(string documented, string protocol, int port, string vpcId, string targetType)
    {
        AssertSameParameters(
            $"Action=CreateTargetGroup&Name=my-targets&{documented}",
            new CreateTargetGroupRequest { Name = "my-targets", Protocol = protocol, Port = port, VpcId = vpcId, TargetType = targetType });
    }

    [Fact]
    public void CreateTargetGroup_Lambda()
    {
        AssertSameParameters(
            "Action=CreateTargetGroup&Name=my-lambda-target&TargetType=lambda",
            new CreateTargetGroupRequest { Name = "my-lambda-target", TargetType = "lambda" });
    }

    [Fact]
    public void CreateLoadBalancer_NetworkWithElasticIps()
    {
        AssertSameParameters(
            "Action=CreateLoadBalancer&Name=my-network-load-balancer&Type=network" +
            "&SubnetMappings.member.1.SubnetId=subnet-8360a9e7&SubnetMappings.member.1.AllocationId=eipalloc-5723d13e" +
            "&SubnetMappings.member.2.SubnetId=subnet-b7d581c0&SubnetMappings.member.2.AllocationId=eipalloc-fc5ca095",
            new CreateLoadBalancerRequest("my-network-load-balancer") {
                Type = "network",
                SubnetMappings = [
                    new SubnetMapping { SubnetId = "subnet-8360a9e7", AllocationId = "eipalloc-5723d13e" },
                    new SubnetMapping { SubnetId = "subnet-b7d581c0", AllocationId = "eipalloc-fc5ca095" }
                ]
            });
    }

    [Fact]
    public void CreateLoadBalancer_SubnetMappingFields()
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_SubnetMapping.html
        AssertSameParameters(
            "Action=CreateLoadBalancer&Name=n&SubnetMappings.member.1.SubnetId=s&SubnetMappings.member.1.PrivateIPv4Address=10.0.0.1" +
            "&SubnetMappings.member.1.IPv6Address=2001:db8::1&SubnetMappings.member.1.SourceNatIpv6Prefix=auto_assigned",
            new CreateLoadBalancerRequest("n") {
                SubnetMappings = [
                    new SubnetMapping { SubnetId = "s", PrivateIPv4Address = "10.0.0.1", IPv6Address = "2001:db8::1", SourceNatIpv6Prefix = "auto_assigned" }
                ]
            });
    }

    [Fact]
    public void RegisterTargets_AvailabilityZone()
    {
        AssertSameParameters(
            $"Action=RegisterTargets&TargetGroupArn={TgArn}&Targets.member.1.Id=10.0.42.17&Targets.member.2.Id=192.168.0.3&Targets.member.2.AvailabilityZone=all",
            new RegisterTargetsRequest(TgArn, new TargetDescription("10.0.42.17"), new TargetDescription("192.168.0.3") { AvailabilityZone = "all" }));
    }
}
