namespace Amazon.Elb.Tests
{
    public class DeleteRuleRequestTests
    {
        [Fact]
        public void Serialize()
        {
            var request = new DeleteRuleRequest("arn");

            Assert.Equal("arn", request.RuleArn);

            Assert.Equal("Action=DeleteRule&RuleArn=arn", Serializer.Serialize(request));
        }
    }
}