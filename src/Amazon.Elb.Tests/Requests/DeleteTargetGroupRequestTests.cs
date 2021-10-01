namespace Amazon.Elb.Tests
{
    public class DeleteTargetGroupRequestTests
    {
        [Fact]
        public void Serialize()
        {
            var request = new DeleteTargetGroupRequest("arn");

            Assert.Equal("Action=DeleteTargetGroup&TargetGroupArn=arn", Serializer.Serialize(request));
        }   
    }
}