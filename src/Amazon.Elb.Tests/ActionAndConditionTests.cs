using System.Text;

namespace Amazon.Elb.Tests;

// Field names and types from
// https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_Action.html
// https://docs.aws.amazon.com/elasticloadbalancing/latest/APIReference/API_RuleCondition.html
public class ActionAndConditionTests
{
    private static void AssertSameParameters(string expected, IElbRequest request)
    {
        Assert.Equal(
            expected.Split('&').Order(StringComparer.Ordinal),
            Serializer.Serialize(request).Split('&').Order(StringComparer.Ordinal));
    }

    private static string Conditions(params RuleCondition[] conditions)
    {
        var request = new ModifyRuleRequest { RuleArn = "r", Conditions = conditions };

        return Serializer.Serialize(request).Replace("Action=ModifyRule&", "").Replace("&RuleArn=r", "");
    }

    [Fact]
    public void Redirect_HttpToHttps()
    {
        AssertSameParameters(
            "Action=ModifyListener&ListenerArn=l&DefaultActions.member.1.Type=redirect" +
            "&DefaultActions.member.1.RedirectConfig.Protocol=HTTPS&DefaultActions.member.1.RedirectConfig.Port=443" +
            "&DefaultActions.member.1.RedirectConfig.Host=#{host}&DefaultActions.member.1.RedirectConfig.Path=/#{path}" +
            "&DefaultActions.member.1.RedirectConfig.Query=#{query}&DefaultActions.member.1.RedirectConfig.StatusCode=HTTP_301",
            new ModifyListenerRequest {
                ListenerArn = "l",
                DefaultActions = [
                    new Action {
                        Type = "redirect",
                        RedirectConfig = new RedirectActionConfig {
                            Protocol = "HTTPS",
                            Port = "443",
                            Host = "#{host}",
                            Path = "/#{path}",
                            Query = "#{query}",
                            StatusCode = "HTTP_301"
                        }
                    }
                ]
            });
    }

    [Fact]
    public void FixedResponse()
    {
        AssertSameParameters(
            "Action=CreateRule&ListenerArn=l&Priority=1&Actions.member.1.Type=fixed-response&Actions.member.1.Order=1" +
            "&Actions.member.1.FixedResponseConfig.StatusCode=503&Actions.member.1.FixedResponseConfig.ContentType=text/plain" +
            "&Actions.member.1.FixedResponseConfig.MessageBody=down",
            new CreateRuleRequest {
                ListenerArn = "l",
                Priority = 1,
                Conditions = [],
                Actions = [
                    new Action {
                        Type = "fixed-response",
                        Order = 1,
                        FixedResponseConfig = new FixedResponseActionConfig { StatusCode = "503", ContentType = "text/plain", MessageBody = "down" }
                    }
                ]
            });
    }

    [Theory]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    public void Forward_WeightedWithStickiness_EncodesLowercaseBoolean(bool enabled, string expected)
    {
        AssertSameParameters(
            "Action=ModifyRule&RuleArn=r&Actions.member.1.Type=forward" +
            "&Actions.member.1.ForwardConfig.TargetGroups.member.1.TargetGroupArn=a&Actions.member.1.ForwardConfig.TargetGroups.member.1.Weight=80" +
            "&Actions.member.1.ForwardConfig.TargetGroups.member.2.TargetGroupArn=b&Actions.member.1.ForwardConfig.TargetGroups.member.2.Weight=20" +
            $"&Actions.member.1.ForwardConfig.TargetGroupStickinessConfig.Enabled={expected}&Actions.member.1.ForwardConfig.TargetGroupStickinessConfig.DurationSeconds=60",
            new ModifyRuleRequest {
                RuleArn = "r",
                Actions = [
                    new Action {
                        Type = "forward",
                        ForwardConfig = new ForwardActionConfig {
                            TargetGroups = [
                                new TargetGroupTuple { TargetGroupArn = "a", Weight = 80 },
                                new TargetGroupTuple { TargetGroupArn = "b", Weight = 20 }
                            ],
                            TargetGroupStickinessConfig = new TargetGroupStickinessConfig { Enabled = enabled, DurationSeconds = 60 }
                        }
                    }
                ]
            });
    }

    [Fact]
    public void Condition_HostHeader()
    {
        Assert.Equal(
            "Conditions.member.1.Field=host-header&Conditions.member.1.HostHeaderConfig.Values.member.1=a.example.com&Conditions.member.1.HostHeaderConfig.Values.member.2=b.example.com",
            Conditions(new RuleCondition { Field = "host-header", HostHeaderConfig = new HostHeaderConditionConfig { Values = ["a.example.com", "b.example.com"] } }));
    }

    [Fact]
    public void Condition_PathPattern_Regex()
    {
        Assert.Equal(
            "Conditions.member.1.Field=path-pattern&Conditions.member.1.PathPatternConfig.RegexValues.member.1=^/img/",
            Conditions(new RuleCondition { Field = "path-pattern", PathPatternConfig = new PathPatternConditionConfig { RegexValues = ["^/img/"] } }));
    }

    [Fact]
    public void Condition_HttpHeader()
    {
        Assert.Equal(
            "Conditions.member.1.Field=http-header&Conditions.member.1.HttpHeaderConfig.HttpHeaderName=User-Agent&Conditions.member.1.HttpHeaderConfig.Values.member.1=*Chrome*",
            Conditions(new RuleCondition { Field = "http-header", HttpHeaderConfig = new HttpHeaderConditionConfig { HttpHeaderName = "User-Agent", Values = ["*Chrome*"] } }));
    }

    [Fact]
    public void Condition_HttpRequestMethod()
    {
        Assert.Equal(
            "Conditions.member.1.Field=http-request-method&Conditions.member.1.HttpRequestMethodConfig.Values.member.1=GET",
            Conditions(new RuleCondition { Field = "http-request-method", HttpRequestMethodConfig = new HttpRequestMethodConditionConfig { Values = ["GET"] } }));
    }

    [Fact]
    public void Condition_QueryString()
    {
        Assert.Equal(
            "Conditions.member.1.Field=query-string&Conditions.member.1.QueryStringConfig.Values.member.1.Key=version&Conditions.member.1.QueryStringConfig.Values.member.1.Value=v1",
            Conditions(new RuleCondition { Field = "query-string", QueryStringConfig = new QueryStringConditionConfig { Values = [new QueryStringKeyValuePair { Key = "version", Value = "v1" }] } }));
    }

    [Fact]
    public void Condition_SourceIp()
    {
        Assert.Equal(
            "Conditions.member.1.Field=source-ip&Conditions.member.1.SourceIpConfig.Values.member.1=10.0.0.0/8",
            Conditions(new RuleCondition { Field = "source-ip", SourceIpConfig = new SourceIpConditionConfig { Values = ["10.0.0.0/8"] } }));
    }

    [Fact]
    public void Condition_TopLevelRegexValues()
    {
        Assert.Equal(
            "Conditions.member.1.Field=path-pattern&Conditions.member.1.RegexValues.member.1=^/a",
            Conditions(new RuleCondition { Field = "path-pattern", RegexValues = ["^/a"] }));
    }

    [Fact]
    public void RegisterTargets_QuicServerId()
    {
        AssertSameParameters(
            "Action=RegisterTargets&TargetGroupArn=tg&Targets.member.1.Id=i-1&Targets.member.1.QuicServerId=0x0123456789abcdef",
            new RegisterTargetsRequest("tg", new TargetDescription("i-1") { QuicServerId = "0x0123456789abcdef" }));
    }

    [Fact]
    public void DescribeRules_PageSize()
    {
        Assert.Equal("Action=DescribeRules&ListenerArn=l&PageSize=400", Serializer.Serialize(new DescribeRulesRequest { ListenerArn = "l", PageSize = 400 }));
    }

    private const string ActionsXml =
        """
        <member>
          <Type>redirect</Type>
          <Order>1</Order>
          <RedirectConfig>
            <Protocol>HTTPS</Protocol><Port>443</Port><Host>#{host}</Host><Path>/#{path}</Path><Query>#{query}</Query><StatusCode>HTTP_301</StatusCode>
          </RedirectConfig>
        </member>
        <member>
          <Type>fixed-response</Type>
          <FixedResponseConfig><StatusCode>503</StatusCode><ContentType>text/plain</ContentType><MessageBody>down</MessageBody></FixedResponseConfig>
        </member>
        <member>
          <Type>forward</Type>
          <ForwardConfig>
            <TargetGroups>
              <member><TargetGroupArn>a</TargetGroupArn><Weight>80</Weight></member>
              <member><TargetGroupArn>b</TargetGroupArn><Weight>20</Weight></member>
            </TargetGroups>
            <TargetGroupStickinessConfig><Enabled>true</Enabled><DurationSeconds>60</DurationSeconds></TargetGroupStickinessConfig>
          </ForwardConfig>
        </member>
        """;

    private static T Parse<T>(string innerXml)
    {
        string root = typeof(T).Name;

        return ElbSerializer<T>.DeserializeXml(Encoding.UTF8.GetBytes($"""<{root} xmlns="{ElbClient.Namespace}">{innerXml}</{root}>"""));
    }

    [Fact]
    public void Rule_ReadsActionConfigs()
    {
        var response = Parse<ModifyRuleResponse>($"<ModifyRuleResult><Rules><member><RuleArn>r</RuleArn><Actions>{ActionsXml}</Actions></member></Rules></ModifyRuleResult>");

        var rule = Assert.Single(response.ModifyRuleResult.Rules);

        Assert.Equal("r", rule.RuleArn);

        Assert.Equal(1, rule.Actions[0].Order);
        Assert.Equal("HTTPS", rule.Actions[0].RedirectConfig!.Protocol);
        Assert.Equal("443", rule.Actions[0].RedirectConfig!.Port);
        Assert.Equal("HTTP_301", rule.Actions[0].RedirectConfig!.StatusCode);

        Assert.Equal("503", rule.Actions[1].FixedResponseConfig!.StatusCode);
        Assert.Equal("down", rule.Actions[1].FixedResponseConfig!.MessageBody);
        Assert.Null(rule.Actions[1].Order);

        var forward = rule.Actions[2].ForwardConfig!;

        Assert.Equal(["a", "b"], forward.TargetGroups!.Select(static t => t.TargetGroupArn));
        Assert.Equal(20, forward.TargetGroups![1].Weight);
        Assert.True(forward.TargetGroupStickinessConfig!.Enabled);
        Assert.Equal(60, forward.TargetGroupStickinessConfig!.DurationSeconds);
    }

    [Fact]
    public void Listener_ReadsDefaultActionConfigs()
    {
        var response = Parse<DescribeListenersResponse>($"<DescribeListenersResult><Listeners><member><DefaultActions>{ActionsXml}</DefaultActions></member></Listeners></DescribeListenersResult>");

        var actions = response.DescribeListenersResult.Listeners[0].DefaultActions;

        Assert.Equal(1, actions[0].Order);
        Assert.Equal("HTTP_301", actions[0].RedirectConfig.StatusCode);
        Assert.Equal("text/plain", actions[1].FixedResponseConfig.ContentType);
        Assert.Equal(80, actions[2].ForwardConfig.TargetGroups![0].Weight);
    }

    [Fact]
    public void Listener_WithoutProtocol_IsNull()
    {
        var response = Parse<DescribeListenersResponse>("<DescribeListenersResult><Listeners><member><ListenerArn>l</ListenerArn></member></Listeners></DescribeListenersResult>");

        Assert.Null(response.DescribeListenersResult.Listeners[0].Protocol);
    }

    [Fact]
    public void Rule_ReadsConditionConfigs()
    {
        var response = Parse<DescribeRulesResponse>(
            """
            <DescribeRulesResult>
              <Rules>
                <member>
                  <Conditions>
                    <member><Field>host-header</Field><Values><member>a.example.com</member></Values><HostHeaderConfig><Values><member>a.example.com</member></Values></HostHeaderConfig></member>
                    <member><Field>path-pattern</Field><RegexValues><member>^/a</member></RegexValues><PathPatternConfig><RegexValues><member>^/a</member></RegexValues></PathPatternConfig></member>
                    <member><Field>http-header</Field><HttpHeaderConfig><HttpHeaderName>User-Agent</HttpHeaderName><Values><member>*Chrome*</member></Values></HttpHeaderConfig></member>
                    <member><Field>http-request-method</Field><HttpRequestMethodConfig><Values><member>GET</member></Values></HttpRequestMethodConfig></member>
                    <member><Field>query-string</Field><QueryStringConfig><Values><member><Key>version</Key><Value>v1</Value></member></Values></QueryStringConfig></member>
                    <member><Field>source-ip</Field><SourceIpConfig><IpAddressType>ipv4</IpAddressType><Values><member>10.0.0.0/8</member></Values></SourceIpConfig></member>
                  </Conditions>
                </member>
              </Rules>
            </DescribeRulesResult>
            """);

        var c = response.DescribeRulesResult.Rules[0].Conditions;

        Assert.Equal(["a.example.com"], c[0].HostHeaderConfig.Values);
        Assert.Equal(["^/a"], c[1].RegexValues);
        Assert.Equal(["^/a"], c[1].PathPatternConfig.RegexValues);
        Assert.Equal("User-Agent", c[2].HttpHeaderConfig.HttpHeaderName);
        Assert.Equal(["*Chrome*"], c[2].HttpHeaderConfig.Values);
        Assert.Equal(["GET"], c[3].HttpRequestMethodConfig.Values);
        Assert.Equal("version", c[4].QueryStringConfig.Values![0].Key);
        Assert.Equal("v1", c[4].QueryStringConfig.Values![0].Value);
        Assert.Equal("ipv4", c[5].SourceIpConfig.IpAddressType);
        Assert.Equal(["10.0.0.0/8"], c[5].SourceIpConfig.Values);
    }

    [Fact]
    public void Target_ReadsQuicServerIdAndAvailabilityZone()
    {
        var response = Parse<DescribeTargetHealthResponse>(
            """
            <DescribeTargetHealthResult>
              <TargetHealthDescriptions>
                <member><Target><Id>i-1</Id><AvailabilityZone>all</AvailabilityZone><QuicServerId>0x0123456789abcdef</QuicServerId></Target></member>
              </TargetHealthDescriptions>
            </DescribeTargetHealthResult>
            """);

        var target = response.DescribeTargetHealthResult.TargetHealthDescriptions[0].Target;

        Assert.Equal("all", target.AvailabilityZone);
        Assert.Equal("0x0123456789abcdef", target.QuicServerId);
    }
}
