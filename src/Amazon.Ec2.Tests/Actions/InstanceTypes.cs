using Xunit;

namespace Amazon.Ec2.Models.Tests
{
    public class InstanceTypes
    {
        [Fact]
        public void A()
        {
            Assert.Equal(70, InstanceTypeMap.All.Length);
        }
    }
}