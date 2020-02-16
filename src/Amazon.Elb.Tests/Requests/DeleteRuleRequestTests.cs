
using System.Linq;
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

            string data = string.Join('&', RequestHelper.ToParams(request).Select(a => a.Key + "=" + a.Value));

            Assert.Equal("Action=DeleteRule&RuleArn=arn", data);

        }
    }
}