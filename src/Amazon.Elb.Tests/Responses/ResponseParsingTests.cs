using System.Text;

namespace Amazon.Elb.Tests;

public class ResponseParsingTests
{
    private static T Parse<T>(string innerXml)
    {
        string root = typeof(T).Name;

        return ElbSerializer<T>.DeserializeXml(Encoding.UTF8.GetBytes(
            $"""
            <{root} xmlns="{ElbClient.Namespace}">
              {innerXml}
              <ResponseMetadata><RequestId>x</RequestId></ResponseMetadata>
            </{root}>
            """));
    }

    private const string LoadBalancerXml =
        """
        <member>
          <LoadBalancerArn>lb-arn</LoadBalancerArn>
          <Scheme>internal</Scheme>
          <LoadBalancerName>my-lb</LoadBalancerName>
          <VpcId>vpc-1</VpcId>
          <CanonicalHostedZoneId>Z2P70J7EXAMPLE</CanonicalHostedZoneId>
          <CreatedTime>2016-03-25T21:29:48.850Z</CreatedTime>
          <AvailabilityZones>
            <member><SubnetId>subnet-1</SubnetId><ZoneName>us-west-2a</ZoneName></member>
            <member><SubnetId>subnet-2</SubnetId><ZoneName>us-west-2b</ZoneName></member>
          </AvailabilityZones>
          <SecurityGroups><member>sg-1</member></SecurityGroups>
          <DNSName>my-lb.elb.amazonaws.com</DNSName>
          <State><Code>failed</Code><Reason>no capacity</Reason></State>
          <Type>application</Type>
        </member>
        """;

    private static void AssertLoadBalancer(LoadBalancer lb)
    {
        Assert.Equal("lb-arn", lb.LoadBalancerArn);
        Assert.Equal("internal", lb.Scheme);
        Assert.Equal("my-lb", lb.LoadBalancerName);
        Assert.Equal("vpc-1", lb.VpcId);
        Assert.Equal("Z2P70J7EXAMPLE", lb.CanonicalHostedZoneId);
        Assert.Equal(["subnet-1", "subnet-2"], lb.AvailabilityZones.Select(static z => z.SubnetId));
        Assert.Equal("us-west-2b", lb.AvailabilityZones[1].ZoneName);
        Assert.Equal(["sg-1"], lb.SecurityGroups);
        Assert.Equal("my-lb.elb.amazonaws.com", lb.DNSName);
        Assert.Equal("failed", lb.State.Code);
        Assert.Equal("no capacity", lb.State.Reason);
        Assert.Equal("application", lb.Type);
    }

    [Fact]
    public void CreateLoadBalancer()
    {
        var response = Parse<CreateLoadBalancerResponse>(
            $"<CreateLoadBalancerResult><LoadBalancers>{LoadBalancerXml}</LoadBalancers></CreateLoadBalancerResult>");

        AssertLoadBalancer(Assert.Single(response.CreateLoadBalancerResult.LoadBalancers));
    }

    [Fact]
    public void DescribeLoadBalancers()
    {
        var response = Parse<DescribeLoadBalancersResponse>(
            $"<DescribeLoadBalancersResult><LoadBalancers>{LoadBalancerXml}{LoadBalancerXml}</LoadBalancers></DescribeLoadBalancersResult>");

        Assert.Equal(2, response.DescribeLoadBalancersResult.LoadBalancers.Length);

        AssertLoadBalancer(response.DescribeLoadBalancersResult.LoadBalancers[1]);
    }

    [Fact]
    public void DescribeLoadBalancers_EmptyList()
    {
        var response = Parse<DescribeLoadBalancersResponse>("<DescribeLoadBalancersResult><LoadBalancers /></DescribeLoadBalancersResult>");

        Assert.Empty(response.DescribeLoadBalancersResult.LoadBalancers);
    }

    [Fact]
    public void CreateTargetGroup_ReadsAllFields()
    {
        var response = Parse<CreateTargetGroupResponse>(
            """
            <CreateTargetGroupResult>
              <TargetGroups>
                <member>
                  <TargetGroupArn>tg-arn</TargetGroupArn>
                  <HealthCheckEnabled>true</HealthCheckEnabled>
                  <HealthCheckTimeoutSeconds>5</HealthCheckTimeoutSeconds>
                  <HealthCheckPort>traffic-port</HealthCheckPort>
                  <Matcher><HttpCode>200</HttpCode></Matcher>
                  <TargetGroupName>my-targets</TargetGroupName>
                  <HealthCheckProtocol>HTTP</HealthCheckProtocol>
                  <HealthCheckPath>/</HealthCheckPath>
                  <Protocol>TCP_UDP</Protocol>
                  <ProtocolVersion>HTTP1</ProtocolVersion>
                  <Port>80</Port>
                  <VpcId>vpc-1</VpcId>
                  <HealthyThresholdCount>5</HealthyThresholdCount>
                  <HealthCheckIntervalSeconds>30</HealthCheckIntervalSeconds>
                  <UnhealthyThresholdCount>2</UnhealthyThresholdCount>
                  <TargetType>ip</TargetType>
                </member>
              </TargetGroups>
            </CreateTargetGroupResult>
            """);

        var tg = Assert.Single(response.CreateTargetGroupResult.TargetGroups);

        Assert.Equal("tg-arn", tg.TargetGroupArn);
        Assert.True(tg.HealthCheckEnabled);
        Assert.Equal(5, tg.HealthCheckTimeoutSeconds);
        Assert.Equal("traffic-port", tg.HealthCheckPort);
        Assert.Equal("200", tg.Matcher.HttpCode);
        Assert.Equal("my-targets", tg.TargetGroupName);
        Assert.Equal("HTTP", tg.HealthCheckProtocol);
        Assert.Equal("/", tg.HealthCheckPath);
        Assert.Equal(Protocol.TCP_UDP, tg.Protocol);
        Assert.Equal("HTTP1", tg.ProtocolVersion);
        Assert.Equal(80, tg.Port);
        Assert.Equal("vpc-1", tg.VpcId);
        Assert.Equal(5, tg.HealthyThresholdCount);
        Assert.Equal(30, tg.HealthCheckIntervalSeconds);
        Assert.Equal(2, tg.UnhealthyThresholdCount);
        Assert.Equal("ip", tg.TargetType);
    }

    [Fact]
    public void TargetGroup_LambdaHasNoProtocol()
    {
        var response = Parse<DescribeTargetGroupsResponse>(
            """
            <DescribeTargetGroupsResult>
              <TargetGroups>
                <member><TargetGroupArn>tg</TargetGroupArn><TargetType>lambda</TargetType></member>
              </TargetGroups>
            </DescribeTargetGroupsResult>
            """);

        var tg = Assert.Single(response.DescribeTargetGroupsResult.TargetGroups);

        Assert.Null(tg.Protocol);
        Assert.Equal(0, tg.Port);
    }

    [Theory]
    [InlineData("HTTP", Protocol.HTTP)]
    [InlineData("HTTPS", Protocol.HTTPS)]
    [InlineData("TCP", Protocol.TCP)]
    [InlineData("TLS", Protocol.TLS)]
    [InlineData("UDP", Protocol.UDP)]
    [InlineData("TCP_UDP", Protocol.TCP_UDP)]
    [InlineData("GENEVE", Protocol.GENEVE)]
    public void Listener_ParsesProtocol(string value, Protocol expected)
    {
        var response = Parse<CreateListenerResponse>(
            $"<CreateListenerResult><Listeners><member><Protocol>{value}</Protocol></member></Listeners></CreateListenerResult>");

        Assert.Equal(expected, response.CreateListenerResult.Listeners[0].Protocol);
    }

    [Fact]
    public void ModifyListener()
    {
        var response = Parse<ModifyListenerResponse>(
            """
            <ModifyListenerResult>
              <Listeners>
                <member>
                  <LoadBalancerArn>lb</LoadBalancerArn>
                  <Protocol>HTTPS</Protocol>
                  <Port>443</Port>
                  <ListenerArn>l</ListenerArn>
                  <DefaultActions><member><Type>forward</Type><TargetGroupArn>tg</TargetGroupArn></member></DefaultActions>
                </member>
              </Listeners>
            </ModifyListenerResult>
            """);

        var listener = Assert.Single(response.ModifyListenerResult.Listeners);

        Assert.Equal(443, listener.Port);
        Assert.Equal("l", listener.ListenerArn);
        Assert.Equal("tg", listener.DefaultActions[0].TargetGroupArn);
    }

    [Fact]
    public void DescribeTargetHealth()
    {
        var response = Parse<DescribeTargetHealthResponse>(
            """
            <DescribeTargetHealthResult>
              <TargetHealthDescriptions>
                <member>
                  <HealthCheckPort>80</HealthCheckPort>
                  <TargetHealth>
                    <State>unhealthy</State>
                    <Reason>Target.Timeout</Reason>
                    <Description>Request timed out</Description>
                  </TargetHealth>
                  <Target><Id>i-1</Id><Port>8080</Port></Target>
                </member>
                <member>
                  <TargetHealth><State>healthy</State></TargetHealth>
                  <Target><Id>i-2</Id></Target>
                </member>
              </TargetHealthDescriptions>
            </DescribeTargetHealthResult>
            """);

        var descriptions = response.DescribeTargetHealthResult.TargetHealthDescriptions;

        Assert.Equal(2, descriptions.Length);

        Assert.Equal("80", descriptions[0].HealthCheckPort);
        Assert.Equal("unhealthy", descriptions[0].TargetHealth.State);
        Assert.Equal("Target.Timeout", descriptions[0].TargetHealth.Reason);
        Assert.Equal("Request timed out", descriptions[0].TargetHealth.Description);
        Assert.Equal("i-1", descriptions[0].Target.Id);
        Assert.Equal(8080, descriptions[0].Target.Port);

        Assert.Equal("healthy", descriptions[1].TargetHealth.State);
        Assert.Null(descriptions[1].TargetHealth.Reason);
        Assert.Equal("i-2", descriptions[1].Target.Id);
        Assert.Null(descriptions[1].Target.Port);
    }

    [Fact]
    public void DescribeTags()
    {
        var response = Parse<DescribeTagsResponse>(
            """
            <DescribeTagsResult>
              <TagDescriptions>
                <member>
                  <ResourceArn>a</ResourceArn>
                  <Tags>
                    <member><Key>k1</Key><Value>v1</Value></member>
                    <member><Key>k2</Key></member>
                  </Tags>
                </member>
              </TagDescriptions>
            </DescribeTagsResult>
            """);

        var description = Assert.Single(response.DescribeTagsResult.TagDescriptions);

        Assert.Equal("a", description.ResourceArn);
        Assert.Equal("v1", description.Tags[0].Value);
        Assert.Equal("k2", description.Tags[1].Key);
        Assert.Null(description.Tags[1].Value);
    }

    private const string AttributesXml =
        """
        <Attributes>
          <member><Value>600</Value><Key>deregistration_delay.timeout_seconds</Key></member>
          <member><Key>stickiness.enabled</Key><Value>false</Value></member>
        </Attributes>
        """;

    [Fact]
    public void TargetGroupAttributes()
    {
        var modified = Parse<ModifyTargetGroupAttributesResponse>($"<ModifyTargetGroupAttributesResult>{AttributesXml}</ModifyTargetGroupAttributesResult>")
            .ModifyTargetGroupAttributesResult.Attributes;

        var described = Parse<DescribeTargetGroupAttributesResponse>($"<DescribeTargetGroupAttributesResult>{AttributesXml}</DescribeTargetGroupAttributesResult>")
            .DescribeTargetGroupAttributesResult.Attributes;

        foreach (var attributes in new[] { modified, described })
        {
            Assert.Equal(2, attributes.Length);
            Assert.Equal("deregistration_delay.timeout_seconds", attributes[0].Key);
            Assert.Equal("600", attributes[0].Value);
        }
    }

    [Fact]
    public void LoadBalancerAttributes()
    {
        var modified = Parse<ModifyLoadBalancerAttributesResponse>($"<ModifyLoadBalancerAttributesResult>{AttributesXml}</ModifyLoadBalancerAttributesResult>")
            .ModifyLoadBalancerAttributesResult.Attributes;

        var described = Parse<DescribeLoadBalancerAttributesResponse>($"<DescribeLoadBalancerAttributesResult>{AttributesXml}</DescribeLoadBalancerAttributesResult>")
            .DescribeLoadBalancerAttributesResult.Attributes;

        foreach (var attributes in new[] { modified, described })
        {
            Assert.Equal(2, attributes.Length);
            Assert.Equal("stickiness.enabled", attributes[1].Key);
            Assert.Equal("false", attributes[1].Value);
        }
    }

    private const string NumberedRuleXml =
        """
        <member>
          <IsDefault>false</IsDefault>
          <Conditions>
            <member>
              <Field>path-pattern</Field>
              <Values><member>/img/*</member></Values>
            </member>
          </Conditions>
          <Priority>10</Priority>
          <Actions><member><Type>forward</Type><TargetGroupArn>tg</TargetGroupArn></member></Actions>
          <RuleArn>r1</RuleArn>
        </member>
        """;

    [Fact]
    public void DescribeRules_NumberedRule()
    {
        var response = Parse<DescribeRulesResponse>($"<DescribeRulesResult><Rules>{NumberedRuleXml}</Rules></DescribeRulesResult>");

        var rule = Assert.Single(response.DescribeRulesResult.Rules);

        Assert.False(rule.IsDefault);
        Assert.Equal("10", rule.Priority);
        Assert.Equal("r1", rule.RuleArn);
        Assert.Equal(["/img/*"], rule.Conditions[0].Values);
    }

    [Fact]
    public void SetRulePriorities()
    {
        var response = Parse<SetRulePrioritiesResponse>($"<SetRulePrioritiesResult><Rules>{NumberedRuleXml}</Rules></SetRulePrioritiesResult>");

        Assert.Equal("r1", Assert.Single(response.SetRulePrioritiesResult.Rules).RuleArn);
    }

    [Fact]
    public void SetSecurityGroups()
    {
        var response = Parse<SetSecurityGroupsResponse>(
            "<SetSecurityGroupsResult><SecurityGroupIds><member>sg-1</member><member>sg-2</member></SecurityGroupIds></SetSecurityGroupsResult>");

        Assert.Equal(["sg-1", "sg-2"], response.SetSecurityGroupsResult.SecurityGroupIds);
    }

    [Fact]
    public void DescribeSSLPolicies_ReadsCipherPriorities()
    {
        var response = Parse<DescribeSSLPoliciesResponse>(
            """
            <DescribeSSLPoliciesResult>
              <SslPolicies>
                <member>
                  <Ciphers><member><Name>AES256-SHA</Name><Priority>19</Priority></member></Ciphers>
                </member>
              </SslPolicies>
            </DescribeSSLPoliciesResult>
            """);

        var cipher = Assert.Single(response.DescribeSSLPoliciesResult.SslPolicies[0].Ciphers);

        Assert.Equal("AES256-SHA", cipher.Name);
        Assert.Equal(19, cipher.Priority);
    }

    [Fact]
    public void UnknownElements_AreIgnored()
    {
        var response = Parse<DescribeLoadBalancersResponse>(
            """
            <DescribeLoadBalancersResult>
              <LoadBalancers>
                <member><LoadBalancerArn>a</LoadBalancerArn><SomethingNew><member>x</member></SomethingNew></member>
              </LoadBalancers>
            </DescribeLoadBalancersResult>
            """);

        Assert.Equal("a", response.DescribeLoadBalancersResult.LoadBalancers[0].LoadBalancerArn);
    }

    [Fact]
    public void DescribeRules_DefaultRule()
    {
        // sample from https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_DescribeRules.html
        var response = Parse<DescribeRulesResponse>(
            $"""
            <DescribeRulesResult>
              <Rules>
                {NumberedRuleXml}
                <member>
                  <IsDefault>true</IsDefault>
                  <Conditions />
                  <Priority>default</Priority>
                  <Actions><member><Type>forward</Type><TargetGroupArn>tg</TargetGroupArn></member></Actions>
                  <RuleArn>r2</RuleArn>
                </member>
              </Rules>
            </DescribeRulesResult>
            """);

        Assert.Equal(2, response.DescribeRulesResult.Rules.Length);
        Assert.True(response.DescribeRulesResult.Rules[1].IsDefault);
        Assert.Equal("default", response.DescribeRulesResult.Rules[1].Priority);
    }

    [Fact]
    public void LoadBalancer_ReadsCreatedTime()
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_LoadBalancer.html
        var response = Parse<DescribeLoadBalancersResponse>(
            $"<DescribeLoadBalancersResult><LoadBalancers>{LoadBalancerXml}</LoadBalancers></DescribeLoadBalancersResult>");

        Assert.Equal(new DateTime(2016, 3, 25, 21, 29, 48, 850, DateTimeKind.Utc), response.DescribeLoadBalancersResult.LoadBalancers[0].CreatedTime.ToUniversalTime());
    }

    [Fact]
    public void DescribeResults_ExposeNextMarker()
    {
        Assert.Equal("m", Parse<DescribeLoadBalancersResponse>("<DescribeLoadBalancersResult><NextMarker>m</NextMarker></DescribeLoadBalancersResult>").DescribeLoadBalancersResult.NextMarker);
        Assert.Equal("m", Parse<DescribeListenersResponse>("<DescribeListenersResult><NextMarker>m</NextMarker></DescribeListenersResult>").DescribeListenersResult.NextMarker);
        Assert.Equal("m", Parse<DescribeRulesResponse>("<DescribeRulesResult><NextMarker>m</NextMarker></DescribeRulesResult>").DescribeRulesResult.NextMarker);
        Assert.Equal("m", Parse<DescribeTargetGroupsResponse>("<DescribeTargetGroupsResult><NextMarker>m</NextMarker></DescribeTargetGroupsResult>").DescribeTargetGroupsResult.NextMarker);
        Assert.Equal("m", Parse<DescribeSSLPoliciesResponse>("<DescribeSSLPoliciesResult><NextMarker>m</NextMarker></DescribeSSLPoliciesResult>").DescribeSSLPoliciesResult.NextMarker);
    }

    [Fact]
    public void DescribeResults_NextMarkerIsNullOnLastPage()
    {
        Assert.Null(Parse<DescribeRulesResponse>("<DescribeRulesResult><Rules /></DescribeRulesResult>").DescribeRulesResult.NextMarker);
    }

    [Theory]
    [InlineData("QUIC")]
    [InlineData("TCP_QUIC")]
    public void TargetGroup_ParsesQuicProtocols(string value)
    {
        // https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_CreateTargetGroup.html
        var response = Parse<DescribeTargetGroupsResponse>(
            $"<DescribeTargetGroupsResult><TargetGroups><member><Protocol>{value}</Protocol></member></TargetGroups></DescribeTargetGroupsResult>");

        Assert.Equal(value, response.DescribeTargetGroupsResult.TargetGroups[0].Protocol.ToString());
    }
}
