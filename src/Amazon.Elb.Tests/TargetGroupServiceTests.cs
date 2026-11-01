using System.Net;

using Amazon.Elb.Exceptions;

namespace Amazon.Elb.Tests;

public class TargetGroupServiceTests
{
    [Fact]
    public async Task AddInstances_RegistersEachInstanceWithoutPort()
    {
        var handler = new FakeElbHandler(FakeElb.EmptyResponse("RegisterTargetsResponse"));

        await new TargetGroupService(FakeElb.Create(handler)).AddInstancesAsync("tg-arn", "i-1", "i-2");

        var request = Assert.Single(handler.Requests);

        Assert.Equal(
            [
                new("Action", "RegisterTargets"),
                new("TargetGroupArn", "tg-arn"),
                new("Targets.member.1.Id", "i-1"),
                new("Targets.member.2.Id", "i-2"),
                new("Version", ElbClient.Version)
            ],
            request.Parameters);
    }

    [Fact]
    public async Task RemoveInstances_DeregistersEachInstanceWithoutPort()
    {
        var handler = new FakeElbHandler(FakeElb.EmptyResponse("DeregisterTargetsResponse"));

        await new TargetGroupService(FakeElb.Create(handler)).RemoveInstancesAsync("tg-arn", "i-1", "i-2");

        var request = Assert.Single(handler.Requests);

        Assert.Equal(
            [
                new("Action", "DeregisterTargets"),
                new("TargetGroupArn", "tg-arn"),
                new("Targets.member.1.Id", "i-1"),
                new("Targets.member.2.Id", "i-2"),
                new("Version", ElbClient.Version)
            ],
            request.Parameters);
    }

    [Fact]
    public async Task InvalidArguments_ThrowWithoutSending()
    {
        var handler = new FakeElbHandler(FakeElb.EmptyResponse("RegisterTargetsResponse"));
        var service = new TargetGroupService(FakeElb.Create(handler));

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddInstancesAsync("arn", null!));
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.RemoveInstancesAsync("arn", null!));

        await Assert.ThrowsAsync<ArgumentException>(() => service.AddInstancesAsync("arn"));
        await Assert.ThrowsAsync<ArgumentException>(() => service.RemoveInstancesAsync("arn"));

        await Assert.ThrowsAsync<ArgumentException>(() => service.AddInstancesAsync("arn", ""));
        await Assert.ThrowsAsync<ArgumentException>(() => service.RemoveInstancesAsync("", "i-1"));

        Assert.Empty(handler.Requests);
    }

    [Fact]
    public async Task ServiceError_Propagates()
    {
        var handler = new FakeElbHandler(
            $"""<ErrorResponse xmlns="{ElbClient.Namespace}"><Error><Type>Sender</Type><Code>InvalidTarget</Code><Message>m</Message></Error></ErrorResponse>""",
            HttpStatusCode.BadRequest);

        var service = new TargetGroupService(FakeElb.Create(handler));

        var ex = await Assert.ThrowsAsync<ElbException>(() => service.AddInstancesAsync("arn", "i-1"));

        Assert.Equal("InvalidTarget", ex.Error.Code);
    }

    [Fact]
    public void Constructor_RequiresClient()
    {
        Assert.Throws<ArgumentNullException>(() => new TargetGroupService(null!));
    }
}
