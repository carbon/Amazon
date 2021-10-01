using System.Linq;

namespace Amazon.Ec2.Tests
{
    public class DescribeVolumesRequestTests
    {
        [Fact]
        public void Serialize()
        {
            var request = new DescribeVolumesRequest(new[] { "a", "b", "c" });

            string data = string.Join('&', request.ToParams().Select(a => a.Key + "=" + a.Value));

            Assert.Equal("Action=DescribeVolumes&VolumeId.1=a&VolumeId.2=b&VolumeId.3=c", data);
        }

        [Fact]
        public void SerializeEmpty()
        {
            var request = new DescribeVolumesRequest();

            string data = string.Join('&', request.ToParams().Select(a => a.Key + "=" + a.Value));

            Assert.Equal("Action=DescribeVolumes", data);
        }
    }
}
