#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;

using Amazon.Metadata;

namespace Amazon;

public sealed class InstanceRoleCredential : IAwsCredential
{
    private int renewCount = 0;

    private readonly SemaphoreSlim gate = new(1, 1);

    public InstanceRoleCredential() { }

    public InstanceRoleCredential(string roleName!!)
    {
        RoleName = roleName;
    }

    internal InstanceRoleCredential(string roleName!!, IamSecurityCredentials credential)
    {
        RoleName = roleName;

        Set(credential);
    }

    public string RoleName { get; internal set; }

    public string AccessKeyId { get; internal set; }

    public string SecretAccessKey { get; internal set; }

    public string SecurityToken { get; internal set; }

    public DateTime Expires { get; internal set; }

    public int RenewCount => renewCount;

    #region Helpers

    public TimeSpan ExpiresIn => Expires - DateTime.UtcNow;

    public bool IsExpired => DateTime.UtcNow < Expires;

    public bool ShouldRenew
    {
        get => RoleName is null || Expires == default || ExpiresIn <= TimeSpan.FromMinutes(5);
    }

    private void Set(IamSecurityCredentials credential)
    {
        AccessKeyId = credential.AccessKeyId;
        SecretAccessKey = credential.SecretAccessKey;
        SecurityToken = credential.Token;
        Expires = credential.Expiration;
    }

    #endregion

    // aws note: refresh 5 minutes before expiration

    public async Task<bool> RenewAsync()
    {
        if (!ShouldRenew) return false;

        // Lock so we only renew the credentials once
        if (!(await gate.WaitAsync(2_000).ConfigureAwait(false)))
        {
            throw new TimeoutException("Timeout waiting for mutex to renew credentials (2s)");
        }

        try
        {
            if (ShouldRenew)
            {
                RoleName ??= await InstanceMetadataService.Instance.GetIamRoleNameAsync().ConfigureAwait(false);

                if (RoleName is null)
                {
                    throw new Exception("The instance is not configured with an IAM role");
                }

                var iamCredential = await InstanceMetadataService.Instance.GetIamSecurityCredentialsAsync(RoleName).ConfigureAwait(false);

                Set(iamCredential);

                Interlocked.Increment(ref renewCount);
            }
        }
        finally
        {
            gate.Release();
        }

        return true;
    }

    public static InstanceRoleCredential Get()
    {
        var ims = InstanceMetadataService.Instance;

        string roleName = ims.GetIamRoleName();

        var iamCredential = ims.GetIamSecurityCredentials(roleName);

        return new InstanceRoleCredential(roleName, iamCredential);
    }

    public static async Task<InstanceRoleCredential> GetAsync()
    {
        var ims = InstanceMetadataService.Instance;

        string roleName = await ims.GetIamRoleNameAsync().ConfigureAwait(false);

        var iamCredential = await ims.GetIamSecurityCredentialsAsync(roleName).ConfigureAwait(false);

        return new InstanceRoleCredential(roleName, iamCredential);
    }

    public static async Task<InstanceRoleCredential> GetAsync(string roleName)
    {
        var iamCredential = await InstanceMetadataService.Instance.GetIamSecurityCredentialsAsync(roleName).ConfigureAwait(false);

        return new InstanceRoleCredential(roleName, iamCredential);
    }
}