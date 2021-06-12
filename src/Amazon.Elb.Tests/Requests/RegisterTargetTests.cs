
using Xunit;

namespace Amazon.Elb.Tests
{
    public class RegisterTargetTests
    {
        [Fact]
        public void Serialize()
        {
            var registerTargets = new RegisterTargetsRequest {
                TargetGroupArn = "arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067",
                Targets = new[] {
                    new TargetDescription { Id = "i-80c8dd94", Port = 1 },
                    new TargetDescription { Id = "i-ceddcd4d" }
                }
            };

            Assert.Equal("Action=RegisterTargets&TargetGroupArn=arn:aws:elasticloadbalancing:us-west-2:123456789012:targetgroup/my-targets/73e2d6bc24d8a067&Targets.member.1.Id=i-80c8dd94&Targets.member.1.Port=1&Targets.member.2.Id=i-ceddcd4d", Serializer.Serialize(registerTargets));
        }

        [Fact]
        public void SerializeRequest_FromConstructor()
        {
            var registerTargets = new RegisterTargetsRequest(
                targetGroupArn : "target-arn",
                targets         : new[] {
                    new TargetDescription("i-80c8dd94", 1),
                    new TargetDescription("i-ceddcd4d")
                }
            );

            Assert.Equal("Action=RegisterTargets&TargetGroupArn=target-arn&Targets.member.1.Id=i-80c8dd94&Targets.member.1.Port=1&Targets.member.2.Id=i-ceddcd4d", Serializer.Serialize(registerTargets));
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