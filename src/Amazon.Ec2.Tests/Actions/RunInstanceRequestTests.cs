using System.Linq;

using Xunit;

namespace Amazon.Ec2.Tests
{
    public class RunInstanceRequestTests
    {
        [Fact]
        public void SerializeRequest()
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

            Assert.Equal("Action=RunInstances&BlockDeviceMapping.1.DeviceName=dev1&BlockDeviceMapping.1.Ebs.VolumeSize=100&ClientToken=1&ImageId=ami-1&InstanceType=&MaxCount=3&MinCount=1&Placement.AvailabilityZone=us-east-1a&SecurityGroupId.1=sg1&SecurityGroupId.2=sg2&SecurityGroupId.3=sg3&SubnetId=subnet-1", Serialize(request));
        }

        [Fact]
        public void SerializeRequest_MetadataOptions()
        {
            var request = new RunInstancesRequest(instanceType: "it", imageId: "ami1") {
                MetadataOptions = new InstanceMetadataOptionsRequest {  HttpTokens = "required" }
            };

            Assert.Equal(1, request.MinCount);
            Assert.Equal(1, request.MaxCount);

            Assert.Equal("Action=RunInstances&ImageId=ami1&InstanceType=it&MaxCount=1&MinCount=1&MetadataOptions.HttpTokens=required", Serialize(request));
        }

        [Fact]
        public void SerializeRequest_MetadataOptions2()
        {
            var request = new RunInstancesRequest(
                imageId         : "ami1",
                instanceType    : null,
                minCount        : 3,
                maxCount        : 20,
                metadataOptions : InstanceMetadataOptionsRequest.RequireHttpToken
            );

            Assert.Equal("Action=RunInstances&ImageId=ami1&MaxCount=20&MinCount=3&MetadataOptions.HttpTokens=required", Serialize(request));
        }

        private static string Serialize(RunInstancesRequest request)
        {
            return string.Join('&', request.ToParams().Select(a => a.Key + "=" + a.Value));
        }

        [Fact]
        public void SerializeRequest2()
        {
            var request = new RunInstancesRequest {
              TagSpecifications = new[] {
                  new TagSpecification("instance", new[] { new Tag("webserver", "production") })
              }
            };

            Assert.Equal("Action=RunInstances&MaxCount=0&MinCount=0&TagSpecification.1.Tag.1.Key=webserver&TagSpecification.1.Tag.1.Value=production&TagSpecification.1.ResourceType=instance", Serialize(request));
        }

        [Fact]
        public void SerializeRequest4()
        {
            var request = new RunInstancesRequest {
                IamInstanceProfile = new IamInstanceProfileSpecification("hello")
            };

            Assert.Equal("Action=RunInstances&IamInstanceProfile.Name=hello&MaxCount=0&MinCount=0", Serialize(request));

            request = new RunInstancesRequest
            {
                IamInstanceProfile = new IamInstanceProfileSpecification("arn:hello")
            };

            Assert.Equal("Action=RunInstances&IamInstanceProfile.Arn=arn:hello&MaxCount=0&MinCount=0", Serialize(request));
        }
    }
}
