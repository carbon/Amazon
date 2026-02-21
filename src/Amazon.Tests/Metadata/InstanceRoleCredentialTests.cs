using Amazon.Metadata;

namespace Amazon.Security.Tests;

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
        var credential = new InstanceRoleCredential("role-name");

        Assert.Equal("role-name", credential.RoleName);
        Assert.Equal(0, credential.RenewCount);
        Assert.Equal(default, credential.Expires);

        Assert.True(credential.ShouldRenew);
    }

    [Fact]
    public void ShouldRenewWhenExpiringInLessThan5Minutes()
    {
        var expiration = DateTime.UtcNow.AddMinutes(3);

        var credential = new InstanceRoleCredential("role-name", new IamSecurityCredentials {
            AccessKeyId = "access-key-id",
            SecretAccessKey = "secret-access-key",
            Code = "Success",
            Expiration = expiration
        });

        Assert.True(credential.ShouldRenew);
    }

    [Fact]
    public void ActiveCredentialDoesNotNeedRenewal()
    {
        var expiration = DateTime.UtcNow.AddMinutes(10);

        var credential = new InstanceRoleCredential("role-name", new IamSecurityCredentials {
            AccessKeyId = "access-key-id",
            SecretAccessKey = "secret-access-key",
            Code = "Success",
            Expiration = expiration,
            LastUpdated = DateTime.UtcNow,
            Token = "security-token",
            Type = "type"
        });

        Assert.Equal("role-name",         credential.RoleName);
        Assert.Equal(0,                   credential.RenewCount);
        Assert.Equal(expiration,          credential.Expires);
        Assert.Equal("access-key-id",     credential.AccessKeyId);
        Assert.Equal("secret-access-key", credential.SecretAccessKey);
        Assert.Equal("security-token",    credential.SecurityToken);
        
        Assert.False(credential.ShouldRenew);
    }

    [Fact]
    public void IsExpiredWhenExpirationIsInThePast()
    {
        var credential = new InstanceRoleCredential("role-name", new IamSecurityCredentials {
            AccessKeyId = "access-key-id",
            SecretAccessKey = "secret-access-key",
            Code = "Success",
            Expiration = DateTime.UtcNow.AddMinutes(-1)
        });

        Assert.True(credential.IsExpired);
    }

    [Fact]
    public void IsNotExpiredWhenExpirationIsInTheFuture()
    {
        var credential = new InstanceRoleCredential("role-name", new IamSecurityCredentials {
            AccessKeyId = "access-key-id",
            SecretAccessKey = "secret-access-key",
            Code = "Success",
            Expiration = DateTime.UtcNow.AddMinutes(1)
        });

        Assert.False(credential.IsExpired);
    }
}