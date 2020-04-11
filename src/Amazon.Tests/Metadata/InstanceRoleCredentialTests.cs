using Xunit;

namespace Amazon.Security.Tests
{
    public class InstanceRoleCredentialTests
    {
        [Fact]
        public void EmptyRoleNeedsRenewed()
        {
            var credential = new InstanceRoleCredential();

            Assert.Equal(0, credential.RenewCount);

            Assert.True(credential.ShouldRenew);
        }

        [Fact]
        public void EmptyExpiresNeedsRenewed()
        {
            var credential = new InstanceRoleCredential("roleName");

            Assert.Equal("roleName", credential.RoleName);
            Assert.Equal(0, credential.RenewCount);
            Assert.True(credential.Expires == default);

            Assert.True(credential.ShouldRenew);
        }
    }
}