namespace Amazon.Ec2.Tests;

public class RunInstanceRequestTests
{
    [Fact]
    public void CanConstruct()
    {
        var request = new RunInstancesRequest(null, null, 1, 100);

        Assert.Equal(1,   request.MinCount);
        Assert.Equal(100, request.MaxCount);
    }

    [Fact]
    public void ThrowsWhenMaxCountGreaterThan100()
    {
        Assert.Throws<ArgumentException>(() => new RunInstancesRequest(null, null, 1, 101));
    }

    [Fact]
    public void ThrowsWhenMinCountLessThan1()
    {
        Assert.Throws<ArgumentException>(() => new RunInstancesRequest(null, null, 0, 100));
    }

    [Fact]
    public void CanSerializeRequest()
    {
        var request = new RunInstancesRequest {
            ClientToken = "1",
            InstanceType = "",
            ImageId = "ami-1",
            MinCount = 1,
            MaxCount = 3,
            Placement = new Placement(availabilityZone: "us-east-1a"),
            BlockDeviceMappings = new[] {
                new BlockDeviceMapping {
                    DeviceName = "dev1",
                    Ebs = new EbsBlockDevice(volumeSize: 100)
                }
            },
            SecurityGroupIds = new[] { "sg1", "sg2", "sg3" },
            SubnetId = "subnet-1"
        };

        Assert.Equal("Action=RunInstances&BlockDeviceMapping.1.DeviceName=dev1&BlockDeviceMapping.1.Ebs.VolumeSize=100&ClientToken=1&ImageId=ami-1&InstanceType=&MaxCount=3&MinCount=1&Placement.AvailabilityZone=us-east-1a&SecurityGroupId.1=sg1&SecurityGroupId.2=sg2&SecurityGroupId.3=sg3&SubnetId=subnet-1", request.Serialize());
    }

    [Fact]
    public void CanSerializeRequest_MetadataOptions()
    {
        var request = new RunInstancesRequest(instanceType: "it", imageId: "ami1") {
            MetadataOptions = new InstanceMetadataOptionsRequest { HttpTokens = "required" }
        };

        Assert.Equal(1, request.MinCount);
        Assert.Equal(1, request.MaxCount);

        Assert.Equal("Action=RunInstances&ImageId=ami1&InstanceType=it&MaxCount=1&MinCount=1&MetadataOptions.HttpTokens=required", request.Serialize());
    }

    [Fact]
    public void CanSerializeRequest_MetadataOptions2()
    {
        var request = new RunInstancesRequest(
            imageId          : "ami1",
            instanceType     : null,
            minCount         : 3,
            maxCount         : 20,
            metadataOptions : InstanceMetadataOptionsRequest.RequireHttpToken
        );

        Assert.Equal("Action=RunInstances&ImageId=ami1&MaxCount=20&MinCount=3&MetadataOptions.HttpTokens=required", request.Serialize());
    }

    [Fact]
    public void CanSerializeRequestWithTags()
    {
        var request = new RunInstancesRequest {
            TagSpecifications = new[] {
                new TagSpecification("instance", new[] { new Tag("webserver", "production") })
            }
        };

        Assert.Equal("Action=RunInstances&MaxCount=0&MinCount=0&TagSpecification.1.Tag.1.Key=webserver&TagSpecification.1.Tag.1.Value=production&TagSpecification.1.ResourceType=instance", request.Serialize());
    }

    [Fact]
    public void CanSerializeRequest4()
    {
        var request = new RunInstancesRequest {
            IamInstanceProfile = new IamInstanceProfileSpecification("hello")
        };

        Assert.Equal("Action=RunInstances&IamInstanceProfile.Name=hello&MaxCount=0&MinCount=0", request.Serialize());

        request = new RunInstancesRequest {
            IamInstanceProfile = new IamInstanceProfileSpecification("arn:hello")
        };

        Assert.Equal("Action=RunInstances&IamInstanceProfile.Arn=arn:hello&MaxCount=0&MinCount=0", request.Serialize());
    }
}