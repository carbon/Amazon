
using Xunit;

namespace Amazon.Elb.Tests
{
    public class DeleteRuleRequestTests
    {
        [Fact]
        public void Construct()
        {
            var request = new DeleteRuleRequest("arn");

            Assert.Equal("arn", request.RuleArn);

        }
    }
}