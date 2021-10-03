namespace Amazon.Ec2.Tests
{
    public class StopInstancesRequestTests
    {
        [Fact]
        public void Request()
        {
            var a = new StopInstancesRequest(new[] { "i-1234567890abcdef0" });

            var result = string.Join('&', a.ToParams().Select(pair => pair.Key + "=" + pair.Value));

            Assert.Equal("Action=StopInstances&InstanceId.1=i-1234567890abcdef0", result);
        }
    }
}