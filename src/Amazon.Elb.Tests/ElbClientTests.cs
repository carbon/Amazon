using System.Net;

using Amazon.Elb.Exceptions;
using Amazon.Exceptions;

namespace Amazon.Elb.Tests;

public class ElbClientTests
{
    private static object[] Op<T>(string action, Func<ElbClient, Task<T>> call)
        where T : IElbResponse
    {
        return [action, typeof(T).Name, new Func<ElbClient, Task>(call)];
    }

    public static TheoryData<string, string, Func<ElbClient, Task>> Operations()
    {
        object[][] operations = [
            Op("AddTags", c => c.AddTagsAsync(new AddTagsRequest { ResourceArns = ["a"], Tags = [new Tag("k", "v")] })),
            Op("DescribeTags", c => c.DescribeTagsAsync(new DescribeTagsRequest(["a"]))),
            Op("RemoveTags", c => c.RemoveTagsAsync(new RemoveTagsRequest { ResourceArns = ["a"], TagKeys = ["k"] })),
            Op("CreateTargetGroup", c => c.CreateTargetGroupAsync(new CreateTargetGroupRequest { Name = "n" })),
            Op("DeleteTargetGroup", c => c.DeleteTargetGroupAsync(new DeleteTargetGroupRequest("a"))),
            Op("DeregisterTargets", c => c.DeregisterTargetsAsync(new DeregisterTargetsRequest("a", new TargetDescription("i-1")))),
            Op("DescribeTargetGroupAttributes", c => c.DescribeTargetGroupAttributesAsync(new DescribeTargetGroupAttributesRequest("a"))),
            Op("DescribeTargetGroups", c => c.DescribeTargetGroupsAsync(new DescribeTargetGroupsRequest())),
            Op("DescribeTargetHealth", c => c.DescribeTargetHealthAsync(new DescribeTargetHealthRequest { TargetGroupArn = "a" })),
            Op("ModifyTargetGroupAttributes", c => c.ModifyTargetGroupAttributesAsync(new ModifyTargetGroupAttributesRequest { Attributes = [new TargetGroupAttribute { Key = "k", Value = "v" }], TargetGroupArn = "a" })),
            Op("RegisterTargets", c => c.RegisterTargetsAsync(new RegisterTargetsRequest("a", new TargetDescription("i-1")))),
            Op("ModifyTargetGroup", c => c.ModifyTargetGroupAsync(new ModifyTargetGroupRequest { TargetGroupArn = "a" })),
            Op("CreateListener", c => c.CreateListenerAsync(new CreateListenerRequest { LoadBalancerArn = "a", Protocol = Protocol.HTTP })),
            Op("DeleteListener", c => c.DeleteListenerAsync(new DeleteListenerRequest("a"))),
            Op("DescribeListeners", c => c.DescribeListenersAsync(new DescribeListenersRequest())),
            Op("ModifyListener", c => c.ModifyListenerAsync(new ModifyListenerRequest { ListenerArn = "a" })),
            Op("CreateRule", c => c.CreateRuleAsync(new CreateRuleRequest { Actions = [], Conditions = [], ListenerArn = "a", Priority = 1 })),
            Op("DeleteRule", c => c.DeleteRuleAsync(new DeleteRuleRequest("a"))),
            Op("DescribeRules", c => c.DescribeRulesAsync(new DescribeRulesRequest())),
            Op("ModifyRule", c => c.ModifyRuleAsync(new ModifyRuleRequest { RuleArn = "a" })),
            Op("SetRulePriorities", c => c.SetRulePrioritiesAsync(new SetRulePrioritiesRequest([]))),
            Op("CreateLoadBalancer", c => c.CreateLoadBalancerAsync(new CreateLoadBalancerRequest("n"))),
            Op("DeleteLoadBalancer", c => c.DeleteLoadBalancerAsync(new DeleteLoadBalancerRequest("a"))),
            Op("DescribeLoadBalancerAttributes", c => c.DescribeLoadBalancerAttributesAsync(new DescribeLoadBalancerAttributesRequest("a"))),
            Op("DescribeLoadBalancers", c => c.DescribeLoadBalancersAsync(new DescribeLoadBalancersRequest())),
            Op("ModifyLoadBalancerAttributes", c => c.ModifyLoadBalancerAttributesAsync(new ModifyLoadBalancerAttributesRequest { Attributes = [], LoadBalancerArn = "a" })),
            Op("SetIpAddressType", c => c.SetIpAddressTypeAsync(new SetIpAddressTypeRequest("ipv4", "a"))),
            Op("SetSecurityGroups", c => c.SetSecurityGroupsAsync(new SetSecurityGroupsRequest { LoadBalancerArn = "a", SecurityGroups = ["sg"] })),
            Op("SetSubnets", c => c.SetSubnetsAsync(new SetSubnetsRequest { LoadBalancerArn = "a", Subnets = ["s"] })),
            Op("DescribeSSLPolicies", c => c.DescribeSSLPoliciesAsync(new DescribeSSLPoliciesRequest()))
        ];

        var data = new TheoryData<string, string, Func<ElbClient, Task>>();

        foreach (var operation in operations)
        {
            data.Add((string)operation[0], (string)operation[1], (Func<ElbClient, Task>)operation[2]);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(Operations))]
    public async Task Operation_SendsActionAndVersion_AndParsesItsResponse(string action, string responseRoot, Func<ElbClient, Task> call)
    {
        var handler = new FakeElbHandler(FakeElb.EmptyResponse(responseRoot));

        await call(FakeElb.Create(handler));

        var request = Assert.Single(handler.Requests);

        Assert.Equal(action, request["Action"]);
        Assert.Equal(ElbClient.Version, request["Version"]);
    }

    [Fact]
    public async Task Request_IsSignedFormPostToRegionalEndpoint()
    {
        var handler = new FakeElbHandler(FakeElb.EmptyResponse("DeleteRuleResponse"));
        var client = FakeElb.Create(handler);

        await client.DeleteRuleAsync(new DeleteRuleRequest("arn"));

        var request = handler.Requests[0];

        Assert.Equal("https://elasticloadbalancing.us-east-1.amazonaws.com/", client.Endpoint);
        Assert.Equal(new Uri(client.Endpoint), request.RequestUri);
        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("application/x-www-form-urlencoded", request.ContentType);
        Assert.Contains("/us-east-1/elasticloadbalancing/aws4_request", request.Authorization);
    }

    [Fact]
    public async Task Request_FormEncodesParameterValues()
    {
        var handler = new FakeElbHandler(FakeElb.EmptyResponse("AddTagsResponse"));

        await FakeElb.Create(handler).AddTagsAsync(new AddTagsRequest {
            ResourceArns = ["arn:aws:elb:lb/app/1"],
            Tags = [new Tag("a b", "c&d=e")]
        });

        var request = handler.Requests[0];

        Assert.Contains("ResourceArns.member.1=arn%3Aaws%3Aelb%3Alb%2Fapp%2F1", request.Body);
        Assert.Equal("a b", request["Tags.member.1.Key"]);
        Assert.Equal("c&d=e", request["Tags.member.1.Value"]);
    }

    [Fact]
    public async Task ErrorResponse_ThrowsElbException()
    {
        var handler = new FakeElbHandler(
            $"""
            <ErrorResponse xmlns="{ElbClient.Namespace}">
              <Error>
                <Type>Sender</Type>
                <Code>TargetGroupNotFound</Code>
                <Message>One or more target groups not found</Message>
              </Error>
              <RequestId>x</RequestId>
            </ErrorResponse>
            """, HttpStatusCode.BadRequest);

        var ex = await Assert.ThrowsAsync<ElbException>(() => FakeElb.Create(handler).DeleteTargetGroupAsync(new DeleteTargetGroupRequest("arn")));

        Assert.IsAssignableFrom<AwsException>(ex);
        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.Equal("One or more target groups not found", ex.Message);
        Assert.Equal("Sender", ex.Error.Type);
        Assert.Equal("TargetGroupNotFound", ex.Error.Code);
        Assert.Single(handler.Requests);
    }

    [Theory]
    [InlineData("<html>Service Unavailable</html>")]
    [InlineData("not xml")]
    [InlineData("")]
    [InlineData("<ErrorResponse xmlns=\"urn:other\"><Error><Code>X</Code></Error></ErrorResponse>")]
    [InlineData("<ErrorResponse xmlns=\"http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/\"><RequestId>x</RequestId></ErrorResponse>")]
    public async Task UnrecognizedErrorBody_ThrowsExceptionWithRawBody(string body)
    {
        var handler = new FakeElbHandler(body, HttpStatusCode.ServiceUnavailable);

        var ex = await Assert.ThrowsAsync<Exception>(() => FakeElb.Create(handler).DescribeRulesAsync(new DescribeRulesRequest()));

        Assert.Equal(body, ex.Message);
    }

    [Theory]
    [InlineData("not xml")]
    [InlineData("<SomethingElse xmlns=\"http://elasticloadbalancing.amazonaws.com/doc/2015-12-01/\" />")]
    [InlineData("<DescribeRulesResponse xmlns=\"urn:other\" />")]
    public async Task SuccessWithUnexpectedBody_Throws(string body)
    {
        var handler = new FakeElbHandler(body);

        await Assert.ThrowsAsync<InvalidOperationException>(() => FakeElb.Create(handler).DescribeRulesAsync(new DescribeRulesRequest()));
    }

    [Fact]
    public void Constructor_RequiresRegionAndCredential()
    {
        Assert.Throws<ArgumentNullException>(() => new ElbClient(null!, new AwsCredential("public", "private")));
        Assert.Throws<ArgumentNullException>(() => new ElbClient(AwsRegion.USEast1, null!));
    }
}
