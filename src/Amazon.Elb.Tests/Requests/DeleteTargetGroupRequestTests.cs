using System.Linq;

using Xunit;

namespace Amazon.Elb.Tests
{
    public class DeleteTargetGroupRequestTests
    {
        [Fact]
        public void Serialize()
        {
            var request = new DeleteTargetGroupRequest("arn");

            string data = string.Join('&', RequestHelper.ToParams(request).Select(a => a.Key + "=" + a.Value));

            Assert.Equal("Action=DeleteTargetGroup&TargetGroupArn=arn", data);
        }
    }
}

/*
 https://elasticloadbalancing.amazonaws.com/?Action=RegisterTargets
&TargetGroupArn=arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067
&Targets.member.1.Id=i-80c8dd94
&Targets.member.2.Id=i-ceddcd4d
&Version=2015-12-01
&AUTHPARAMS
*/