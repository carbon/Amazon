namespace Amazon.Elb.Tests;

public class DeleteTargetGroupRequestTests
{
    [Fact]
    public void CanSerialize()
    {
        var request = new DeleteTargetGroupRequest("arn");

        Assert.Equal("Action=DeleteTargetGroup&TargetGroupArn=arn", Serializer.Serialize(request));
    }   
}