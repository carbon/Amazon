using System.Net;

using Amazon.Ec2.Exceptions;

namespace Amazon.Ec2.Tests;

public class Ec2ClientTests
{
    private static (Ec2Client client, FakeEc2Handler handler) Create(params string[] xml)
    {
        var handler = new FakeEc2Handler([.. xml.Select(static x => FakeEc2.Xml(x))]);

        return (FakeEc2.Client(handler), handler);
    }

    private const string StateChange =
        """
        <instancesSet>
          <item>
            <instanceId>i-1</instanceId>
            <currentState><code>0</code><name>pending</name></currentState>
            <previousState><code>80</code><name>stopped</name></previousState>
          </item>
        </instancesSet>
        """;

    [Fact]
    public async Task SendsSignedFormPostWithVersion()
    {
        var (client, handler) = Create(FakeEc2.Response("RebootInstancesResponse", "<return>true</return>"));

        await client.RebootInstancesAsync(new RebootInstancesRequest("i-1"));

        var request = handler.Request;

        Assert.Equal(HttpMethod.Post, request.Method);
        Assert.Equal("https://ec2.us-east-1.amazonaws.com/", request.RequestUri.ToString());
        Assert.Equal("application/x-www-form-urlencoded", request.ContentType);
        Assert.StartsWith("AWS4-HMAC-SHA256 Credential=public/", request.Authorization);
        Assert.Contains("/us-east-1/ec2/aws4_request", request.Authorization);
        Assert.Equal("Action=RebootInstances&InstanceId.1=i-1&Version=2016-11-15", request.Body);
    }

    [Fact]
    public async Task EncodesFilterValuesInTheBody()
    {
        var (client, handler) = Create(FakeEc2.Response("DescribeVpcsResponse", "<vpcSet/>"));

        var request = new DescribeVpcsRequest();

        request.Filters.Add(new Filter("tag:Name", "a b&c=d"));

        await client.DescribeVpcsAsync(request);

        Assert.Contains("Filter.1.Value=a+b%26c%3Dd", handler.Request.Body);
        Assert.Equal("a b&c=d", handler.Request.Form["Filter.1.Value"]);
    }

    [Fact]
    public async Task RunInstancesParsesReservation()
    {
        var (client, handler) = Create(FakeEc2.Response("RunInstancesResponse",
            """
            <reservationId>r-1</reservationId>
            <ownerId>111122223333</ownerId>
            <requesterId>444455556666</requesterId>
            <instancesSet>
              <item>
                <instanceId>i-1</instanceId>
                <imageId>ami-1</imageId>
                <instanceState><code>0</code><name>pending</name></instanceState>
                <instanceType>t2.micro</instanceType>
                <launchTime>2024-01-02T03:04:05.000Z</launchTime>
                <platform>windows</platform>
                <kernelId>aki-1</kernelId>
                <ipAddress>54.0.0.1</ipAddress>
                <vpcId>vpc-1</vpcId>
                <rootDeviceName>/dev/xvda</rootDeviceName>
                <blockDeviceMapping>
                  <item>
                    <deviceName>/dev/xvda</deviceName>
                    <ebs>
                      <volumeId>vol-1</volumeId>
                      <status>attached</status>
                      <attachTime>2024-01-02T03:04:09.000Z</attachTime>
                      <deleteOnTermination>true</deleteOnTermination>
                    </ebs>
                  </item>
                </blockDeviceMapping>
              </item>
            </instancesSet>
            """));

        var response = await client.RunInstancesAsync(new RunInstancesRequest("ami-1", "t2.micro"));

        Assert.Equal("RunInstances", handler.Request.Form["Action"]);

        Assert.Equal("r-1", response.ReservationId);
        Assert.Equal("111122223333", response.OwnerId);
        Assert.Equal("444455556666", response.RequesterId);

        var instance = Assert.Single(response.Instances);

        Assert.Equal("i-1", instance.InstanceId);
        Assert.Equal("ami-1", instance.ImageId);
        Assert.Equal("pending", instance.InstanceState.Name);
        Assert.Equal(new DateTime(2024, 1, 2, 3, 4, 5, DateTimeKind.Utc), instance.LaunchTime.ToUniversalTime());
        Assert.Equal("windows", instance.Platform);
        Assert.Equal("aki-1", instance.KernelId);
        Assert.Equal("54.0.0.1", instance.IpAddress);
        Assert.Equal("vpc-1", instance.VpcId);
        Assert.Equal("/dev/xvda", instance.RootDeviceName);

        var ebs = Assert.Single(instance.BlockDeviceMappings).Ebs;

        Assert.Equal("vol-1", ebs.VolumeId);
        Assert.Equal("attached", ebs.Status);
        Assert.True(ebs.DeleteOnTermination);
        Assert.NotNull(ebs.AttachTime);
    }

    [Fact]
    public async Task StartStopAndTerminateParseStateChanges()
    {
        var (client, handler) = Create(
            FakeEc2.Response("StartInstancesResponse", StateChange),
            FakeEc2.Response("StopInstancesResponse", StateChange),
            FakeEc2.Response("TerminateInstancesResponse", StateChange));

        var started = await client.StartInstancesAsync(new StartInstancesRequest("i-1"));
        var stopped = await client.StopInstancesAsync(new StopInstancesRequest("i-1"));
        var terminated = await client.TerminateInstancesAsync(new TerminateInstancesRequest("i-1"));

        Assert.Equal("pending", Assert.Single(started.Instances).CurrentState.Name);
        Assert.Equal(80, Assert.Single(stopped.Instances).PreviousState.Code);
        Assert.Equal("i-1", Assert.Single(terminated.Instances).InstanceId);

        Assert.Equal(["StartInstances", "StopInstances", "TerminateInstances"], handler.Requests.Select(static r => r.Form["Action"]));
    }

    [Fact]
    public async Task ShortcutsSendTheIdAndReturnTheFirstResult()
    {
        var (client, handler) = Create(
            FakeEc2.Response("DescribeImagesResponse", "<imagesSet><item><imageId>ami-1</imageId></item></imagesSet>"),
            FakeEc2.Response("DescribeSubnetsResponse", "<subnetSet><item><subnetId>subnet-1</subnetId></item></subnetSet>"),
            FakeEc2.Response("DescribeNetworkInterfacesResponse", "<networkInterfaceSet><item><networkInterfaceId>eni-1</networkInterfaceId></item></networkInterfaceSet>"),
            FakeEc2.Response("DescribeInstanceTypesResponse", "<instanceTypeSet><item><instanceType>t2.micro</instanceType></item></instanceTypeSet>"),
            FakeEc2.Response("DescribeVolumesResponse", "<volumeSet><item><volumeId>vol-1</volumeId></item></volumeSet>"),
            FakeEc2.Response("DescribeVpcsResponse", "<vpcSet><item><vpcId>vpc-1</vpcId></item></vpcSet>"));

        Assert.Equal("ami-1", (await client.DescribeImageAsync("ami-1"))!.ImageId);
        Assert.Equal("subnet-1", (await client.DescribeSubnetAsync("subnet-1"))!.SubnetId);
        Assert.Equal("eni-1", (await client.DescribeNetworkInterfaceAsync("eni-1"))!.NetworkInterfaceId);
        Assert.Equal("t2.micro", (await client.DescribeInstanceTypeAsync("t2.micro"))!.InstanceType);
        Assert.Equal("vol-1", (await client.DescribeVolumeAsync("vol-1"))!.VolumeId);
        Assert.Equal("vpc-1", (await client.DescribeVpcAsync("vpc-1"))!.VpcId);

        Assert.Equal("ami-1", handler.Requests[0].Form["ImageId.1"]);
        Assert.Equal("subnet-1", handler.Requests[1].Form["SubnetId.1"]);
        Assert.Equal("eni-1", handler.Requests[2].Form["NetworkInterfaceId.1"]);
        Assert.Equal("t2.micro", handler.Requests[3].Form["InstanceType.1"]);
        Assert.Equal("vol-1", handler.Requests[4].Form["VolumeId.1"]);
        Assert.Equal("vpc-1", handler.Requests[5].Form["VpcId.1"]);
    }

    [Fact]
    public async Task ShortcutsReturnNullWhenNothingMatches()
    {
        var (client, _) = Create(
            FakeEc2.Response("DescribeImagesResponse", "<imagesSet/>"),
            FakeEc2.Response("DescribeSubnetsResponse", "<subnetSet/>"),
            FakeEc2.Response("DescribeNetworkInterfacesResponse", "<networkInterfaceSet/>"),
            FakeEc2.Response("DescribeInstanceTypesResponse", "<instanceTypeSet/>"),
            FakeEc2.Response("DescribeVolumesResponse", "<volumeSet/>"),
            FakeEc2.Response("DescribeVpcsResponse", "<vpcSet/>"));

        Assert.Null(await client.DescribeImageAsync("a"));
        Assert.Null(await client.DescribeSubnetAsync("a"));
        Assert.Null(await client.DescribeNetworkInterfaceAsync("a"));
        Assert.Null(await client.DescribeInstanceTypeAsync("a"));
        Assert.Null(await client.DescribeVolumeAsync("a"));
        Assert.Null(await client.DescribeVpcAsync("a"));
    }

    private static readonly string s_instances = FakeEc2.Response("DescribeInstancesResponse",
        """
        <reservationSet>
          <item><instancesSet><item><instanceId>i-1</instanceId></item></instancesSet></item>
        </reservationSet>
        """);

    [Fact]
    public async Task DescribeInstancesParsesTheResponse()
    {
        var (client, _) = Create(s_instances);

        var response = await client.DescribeInstancesAsync(new DescribeInstancesRequest("i-1"));

        Assert.Equal("i-1", Assert.Single(response.Instances).InstanceId);
    }

    [Fact]
    public async Task DescribeInstanceReturnsTheInstance()
    {
        var (client, handler) = Create(s_instances);

        var instance = await client.DescribeInstanceAsync("i-1");

        Assert.Equal("i-1", instance!.InstanceId);
        Assert.Equal("i-1", handler.Request.Form["InstanceId.1"]);
    }

    [Fact]
    public async Task DescribeInstancesExposesNextToken()
    {
        var (client, handler) = Create(FakeEc2.Response("DescribeInstancesResponse", "<reservationSet/><nextToken>t1</nextToken>"));

        var response = await client.DescribeInstancesAsync(new DescribeInstancesRequest { MaxResults = 5 });

        Assert.Empty(response.Instances);
        Assert.Equal("t1", response.NextToken);
        Assert.Equal("5", handler.Request.Form["MaxResults"]);
    }

    [Fact]
    public async Task DescribeInstanceReturnsNullWhenNothingMatches()
    {
        var (client, _) = Create(FakeEc2.Response("DescribeInstancesResponse", "<reservationSet/>"));

        Assert.Null(await client.DescribeInstanceAsync("i-1"));
    }

    private static string ImagePage(string imageId, string? nextToken)
    {
        return FakeEc2.Response("DescribeImagesResponse",
            $"<imagesSet><item><imageId>{imageId}</imageId></item></imagesSet>" +
            (nextToken is null ? "" : $"<nextToken>{nextToken}</nextToken>"));
    }

    [Fact]
    public async Task DescribeAllImagesFollowsNextTokenUntilAbsent()
    {
        var (client, handler) = Create(ImagePage("ami-1", "t1"), ImagePage("ami-2", "t2"), ImagePage("ami-3", null));

        var request = new DescribeImagesRequest { OwnerIds = ["self"] };

        request.Filters.Add(new Filter("state", "available"));

        var images = await client.DescribeAllImagesAsync(request);

        Assert.Equal(["ami-1", "ami-2", "ami-3"], images.Select(static i => i.ImageId));

        Assert.Equal(3, handler.Requests.Count);

        Assert.False(handler.Requests[0].Form.ContainsKey("NextToken"));
        Assert.Equal("t1", handler.Requests[1].Form["NextToken"]);
        Assert.Equal("t2", handler.Requests[2].Form["NextToken"]);

        Assert.All(handler.Requests, static r => {
            Assert.Equal("1000", r.Form["MaxResults"]);
            Assert.Equal("available", r.Form["Filter.1.Value"]);
        });
    }

    [Fact]
    public async Task DescribeAllImagesStopsAfterTwentyPages()
    {
        // Pins the current cap, which ends the listing without telling the caller that more pages exist
        var (client, handler) = Create(ImagePage("ami-1", "more"));

        var images = await client.DescribeAllImagesAsync(new DescribeImagesRequest());

        Assert.Equal(20, handler.Requests.Count);
        Assert.Equal(20, images.Count);
    }

    [Fact]
    public async Task DescribeInstanceTypesExposesNextToken()
    {
        var (client, _) = Create(FakeEc2.Response("DescribeInstanceTypesResponse", "<instanceTypeSet/><nextToken>t1</nextToken>"));

        var response = await client.DescribeInstanceTypesAsync(new DescribeInstanceTypesRequest { MaxResults = 5 });

        Assert.Equal("t1", response.NextToken);
        Assert.Equal("r-1", response.RequestId);
    }

    [Fact]
    public async Task NonXmlErrorKeepsBodyAndStatus()
    {
        var client = FakeEc2.Client(new FakeEc2Handler(FakeEc2.Xml("Service Unavailable", HttpStatusCode.ServiceUnavailable)));

        var ex = await Assert.ThrowsAsync<Ec2Exception>(() => client.RebootInstancesAsync(new RebootInstancesRequest("i-1")));

        Assert.Equal("Service Unavailable", ex.Message);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, ex.HttpStatusCode);
        Assert.Null(ex.Errors);
    }

    private const string SingleError =
        """
        <Response>
          <Errors><Error><Code>InvalidInstanceID.NotFound</Code><Message>The instance ID 'i-1' does not exist</Message></Error></Errors>
          <RequestID>req-1</RequestID>
        </Response>
        """;

    [Fact]
    public async Task ErrorResponseKeepsStatusAndServiceMessage()
    {
        var client = FakeEc2.Client(new FakeEc2Handler(FakeEc2.Xml(SingleError, HttpStatusCode.BadRequest)));

        var ex = await Assert.ThrowsAsync<Ec2Exception>(() => client.RebootInstancesAsync(new RebootInstancesRequest("i-1")));

        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.Contains("does not exist", ex.Message);
    }

    [Fact]
    public async Task ErrorResponseIsParsedIntoErrors()
    {
        var client = FakeEc2.Client(new FakeEc2Handler(FakeEc2.Xml(SingleError, HttpStatusCode.BadRequest)));

        var ex = await Assert.ThrowsAsync<Ec2Exception>(() => client.RebootInstancesAsync(new RebootInstancesRequest("i-1")));

        Assert.Equal("The instance ID 'i-1' does not exist", ex.Message);
        Assert.Equal("InvalidInstanceID.NotFound", Assert.Single(ex.Errors!).Code);
    }

    [Fact]
    public async Task MultipleErrorsAreAllExposed()
    {
        var client = FakeEc2.Client(new FakeEc2Handler(FakeEc2.Xml(
            """
            <Response>
              <Errors>
                <Error><Code>A</Code><Message>a</Message></Error>
                <Error><Code>B</Code><Message>b</Message></Error>
              </Errors>
              <RequestID>req-1</RequestID>
            </Response>
            """, HttpStatusCode.BadRequest)));

        var ex = await Assert.ThrowsAsync<Ec2Exception>(() => client.RebootInstancesAsync(new RebootInstancesRequest("i-1")));

        Assert.Equal(["A", "B"], ex.Errors!.Select(static e => e.Code));
    }

    [Theory]
    [InlineData("<Response><RequestID>req-1</RequestID></Response>")]
    [InlineData("<Response><Errors/></Response>")]
    [InlineData("<Other/>")]
    public async Task XmlWithoutErrorsKeepsTheBody(string body)
    {
        var client = FakeEc2.Client(new FakeEc2Handler(FakeEc2.Xml(body, HttpStatusCode.InternalServerError)));

        var ex = await Assert.ThrowsAsync<Ec2Exception>(() => client.RebootInstancesAsync(new RebootInstancesRequest("i-1")));

        Assert.Equal(body, ex.Message);
        Assert.Equal(HttpStatusCode.InternalServerError, ex.HttpStatusCode);
        Assert.Null(ex.Errors);
    }
}
