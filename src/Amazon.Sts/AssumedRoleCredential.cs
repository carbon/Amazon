using System.Threading.Tasks;

using Amazon.Sts;

namespace Amazon.Security;

public sealed class AssumedRoleCredential : IAwsCredential
{
    private readonly string _roleArn;
    private readonly string _roleSession;

    private Credentials? _credentials = null;

    private readonly StsClient _stsClient;

    public AssumedRoleCredential(AwsRegion region, IAwsCredential credential, string roleArn, string roleSession)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleArn);
        ArgumentException.ThrowIfNullOrEmpty(roleSession);

        _roleArn = roleArn;
        _roleSession = roleSession;
        _stsClient = new StsClient(region, credential);
    }

    public AssumedRoleCredential(StsClient stsClient, string roleArn, string roleSession)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleArn);
        ArgumentException.ThrowIfNullOrEmpty(roleSession);

        _roleArn = roleArn;
        _roleSession = roleSession;
        _stsClient = stsClient;
    }

    public string AccessKeyId => _credentials?.AccessKeyId!;

    public string SecretAccessKey => _credentials?.SecretAccessKey!;

    public string SecurityToken => _credentials?.SessionToken!;

    public bool ShouldRenew => _credentials is null || _credentials.ExpiresIn < TimeSpan.FromMinutes(5);

    public async Task<bool> RenewAsync()
    {
        var request = new AssumeRoleRequest(_roleArn, _roleSession, TimeSpan.FromMinutes(15));

        var result = await _stsClient.AssumeRoleAsync(request).ConfigureAwait(false);

        _credentials = result.AssumeRoleResult.Credentials;

        return true;
    }
}
