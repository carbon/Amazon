#nullable disable

using System.Text.Json.Serialization;
using System.Threading;

using Amazon.Metadata;

namespace Amazon;

public sealed class InstanceRoleCredential : IAwsCredential
{
    private int renewCount = 0;

    private readonly SemaphoreSlim _gate = new(1, 1);
    private IamSecurityCredentials _credential;

    public InstanceRoleCredential() { }

    public InstanceRoleCredential(string roleName)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        RoleName = roleName;
    }

    internal InstanceRoleCredential(string roleName, IamSecurityCredentials credential)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        RoleName = roleName;
        _credential = credential;
    }

    public string RoleName { get; private set; }

    public string AccessKeyId => _credential.AccessKeyId;

    [JsonIgnore]
    public string SecretAccessKey => _credential.SecretAccessKey;

    [JsonIgnore]
    public string SecurityToken => _credential.Token;

    public DateTime Expires => _credential?.Expiration ?? default;

    public DateTime Modified => _credential?.LastUpdated ?? default;

    public int RenewCount => renewCount;

    #region Helpers

    public TimeSpan ExpiresIn => Expires - DateTime.UtcNow;

    public bool IsExpired => DateTime.UtcNow >= Expires;

    // AWS Recommendation:
    // - refresh 5 minutes before expiration
    public bool ShouldRenew => RoleName is null || _credential is null || Expires <= DateTime.UtcNow.AddMinutes(5);

    #endregion

    public async Task<bool> RenewAsync()
    {
        if (!ShouldRenew) return false;

        RoleName ??= await InstanceMetadataService.Instance.GetIamRoleNameAsync().ConfigureAwait(false);

        if (RoleName is null)
        {
            throw new Exception("The instance is not configured with an IAM role");
        }

        // Lock so we only renew the credentials once
        if (!(await _gate.WaitAsync(2_000).ConfigureAwait(false)))
        {
            throw new TimeoutException("Timeout waiting for mutex to renew credentials (2s)");
        }

        try
        {
            if (ShouldRenew)
            {
                var iamCredential = await InstanceMetadataService.Instance.GetIamSecurityCredentialsAsync(RoleName).ConfigureAwait(false);

                _credential = iamCredential;

                Interlocked.Increment(ref renewCount);
            }
        }
        finally
        {
            _gate.Release();
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
        ArgumentException.ThrowIfNullOrEmpty(roleName);

        var iamCredential = await InstanceMetadataService.Instance.GetIamSecurityCredentialsAsync(roleName).ConfigureAwait(false);

        return new InstanceRoleCredential(roleName, iamCredential);
    }
}