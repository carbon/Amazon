namespace Amazon.Ec2.Tests;

public class InstanceRequestTests
{
    [Fact]
    public void StartInstancesSerializesIdsAndDryRun()
    {
        var request = new StartInstancesRequest("i-1", "i-2") { DryRun = true };

        Assert.Equal("Action=StartInstances&DryRun=True&InstanceId.1=i-1&InstanceId.2=i-2", request.Serialize());
    }

    [Fact]
    public void StopInstancesSerializesForce()
    {
        var request = new StopInstancesRequest("i-1") { Force = true };

        Assert.Equal("Action=StopInstances&Force=True&InstanceId.1=i-1", request.Serialize());
    }

    [Fact]
    public void TerminateInstancesSerializesDryRun()
    {
        var request = new TerminateInstancesRequest("i-1") { DryRun = false };

        Assert.Equal("Action=TerminateInstances&DryRun=False&InstanceId.1=i-1", request.Serialize());
    }

    [Fact]
    public void InstanceActionsRequireIds()
    {
        Assert.Throws<ArgumentNullException>(() => new RebootInstancesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new StartInstancesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new StopInstancesRequest(null!));
        Assert.Throws<ArgumentNullException>(() => new TerminateInstancesRequest(null!));

        Assert.Throws<ArgumentException>(() => new RebootInstancesRequest([]));
        Assert.Throws<ArgumentException>(() => new StartInstancesRequest([]));
        Assert.Throws<ArgumentException>(() => new StopInstancesRequest([]));
        Assert.Throws<ArgumentException>(() => new TerminateInstancesRequest([]));
    }
}

public class RunInstancesParameterTests
{
    private static Dictionary<string, string> Params(IEc2Request request)
    {
        return request.ToParams().ToDictionary(static pair => pair.Key, static pair => pair.Value);
    }

    [Fact]
    public void ConstructorMapsEveryArgumentToItsParameter()
    {
        var parameters = Params(new RunInstancesRequest(
            imageId          : "ami-1",
            instanceType     : "t2.micro",
            minCount         : 2,
            maxCount         : 4,
            keyName          : "key",
            subnetId         : "subnet-1",
            userData         : "dXNlcg==",
            clientToken      : "token",
            metadataOptions  : InstanceMetadataOptionsRequest.RequireHttpToken,
            securityGroupIds : ["sg-1", "sg-2"]));

        Assert.Equal(new Dictionary<string, string> {
            ["Action"] = "RunInstances",
            ["ImageId"] = "ami-1",
            ["InstanceType"] = "t2.micro",
            ["MinCount"] = "2",
            ["MaxCount"] = "4",
            ["KeyName"] = "key",
            ["SubnetId"] = "subnet-1",
            ["UserData"] = "dXNlcg==",
            ["ClientToken"] = "token",
            ["MetadataOptions.HttpTokens"] = "required",
            ["SecurityGroupId.1"] = "sg-1",
            ["SecurityGroupId.2"] = "sg-2"
        }, parameters);
    }

    [Fact]
    public void SerializesScalarOptions()
    {
        var parameters = Params(new RunInstancesRequest {
            MinCount = 1,
            MaxCount = 1,
            InstanceInitiatedShutdownBehavior = "terminate",
            Ipv6AddressCount = 2,
            KernelId = "aki-1",
            PrivateIpAddress = "10.0.0.5"
        });

        Assert.Equal("terminate", parameters["InstanceInitiatedShutdownBehavior"]);
        Assert.Equal("2", parameters["Ipv6AddressCount"]);
        Assert.Equal("aki-1", parameters["KernelId"]);
        Assert.Equal("10.0.0.5", parameters["PrivateIpAddress"]);
    }

    [Fact]
    public void SerializesNestedObjects()
    {
        var parameters = Params(new RunInstancesRequest {
            MinCount = 1,
            MaxCount = 1,
            Monitoring = new RunInstancesMonitoringEnabled(true),
            Placement = new Placement(availabilityZone: "us-east-1a", groupName: "group", hostId: "h-1", tenancy: "dedicated"),
            MetadataOptions = new InstanceMetadataOptionsRequest {
                HttpEndpoint = "enabled",
                HttpPutResponseHopLimit = 2,
                HttpTokens = "optional"
            }
        });

        Assert.True(parameters.ContainsKey("Monitoring.Enabled"));
        Assert.Equal("us-east-1a", parameters["Placement.AvailabilityZone"]);
        Assert.Equal("group", parameters["Placement.GroupName"]);
        Assert.Equal("h-1", parameters["Placement.HostId"]);
        Assert.Equal("dedicated", parameters["Placement.Tenancy"]);
        Assert.Equal("enabled", parameters["MetadataOptions.HttpEndpoint"]);
        Assert.Equal("2", parameters["MetadataOptions.HttpPutResponseHopLimit"]);
        Assert.Equal("optional", parameters["MetadataOptions.HttpTokens"]);
    }

    [Fact]
    public void NumbersBlockDeviceMappingsFromOne()
    {
        var parameters = Params(new RunInstancesRequest {
            MinCount = 1,
            MaxCount = 1,
            BlockDeviceMappings = [
                new BlockDeviceMapping(BlockDeviceNames.Root, new EbsBlockDevice(volumeSize: 8, volumeType: "gp3", iops: 3000, snapshotId: "snap-1")),
                new BlockDeviceMapping { DeviceName = BlockDeviceNames.xvda, VirtualName = "ephemeral0" }
            ]
        });

        Assert.Equal("/dev/sda1", parameters["BlockDeviceMapping.1.DeviceName"]);
        Assert.Equal("8", parameters["BlockDeviceMapping.1.Ebs.VolumeSize"]);
        Assert.Equal("gp3", parameters["BlockDeviceMapping.1.Ebs.VolumeType"]);
        Assert.Equal("3000", parameters["BlockDeviceMapping.1.Ebs.Iops"]);
        Assert.Equal("snap-1", parameters["BlockDeviceMapping.1.Ebs.SnapshotId"]);
        Assert.Equal("/dev/xvda", parameters["BlockDeviceMapping.2.DeviceName"]);
        Assert.Equal("ephemeral0", parameters["BlockDeviceMapping.2.VirtualName"]);
        Assert.False(parameters.ContainsKey("BlockDeviceMapping.2.Ebs.VolumeSize"));
    }

    [Fact]
    public void NumbersTagSpecificationsAndTagsIndependently()
    {
        var parameters = Params(new RunInstancesRequest {
            MinCount = 1,
            MaxCount = 1,
            TagSpecifications = [
                new TagSpecification("instance", new Tag("a", "1"), new Tag("b", "2")),
                new TagSpecification("volume", new Tag("c", "3"))
            ]
        });

        Assert.Equal("instance", parameters["TagSpecification.1.ResourceType"]);
        Assert.Equal("b", parameters["TagSpecification.1.Tag.2.Key"]);
        Assert.Equal("2", parameters["TagSpecification.1.Tag.2.Value"]);
        Assert.Equal("volume", parameters["TagSpecification.2.ResourceType"]);
        Assert.Equal("c", parameters["TagSpecification.2.Tag.1.Key"]);
    }

    [Fact]
    public void AllowsTheFiftyTagsEc2Permits()
    {
        var tags = Enumerable.Range(1, 50).Select(static i => new Tag($"k{i}", $"v{i}")).ToArray();

        var parameters = Params(new RunInstancesRequest {
            ImageId = "ami-1",
            InstanceType = "t2.micro",
            MinCount = 1,
            MaxCount = 1,
            TagSpecifications = [new TagSpecification("instance", tags)]
        });

        Assert.Equal("k50", parameters["TagSpecification.1.Tag.50.Key"]);
    }

    [Fact]
    public void IamProfileRejectsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new IamInstanceProfileSpecification(null!));
    }
}
